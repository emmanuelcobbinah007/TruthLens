using Microsoft.ML;
using Microsoft.ML.Data;
using TruthLens.Api.Data;

namespace TruthLens.Api.ML;

public class ClaimTrainingExample
{
    public string Text { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
}

public class ClaimPrediction
{
    [ColumnName("PredictedLabel")]
    public string PredictedLabel { get; set; } = string.Empty;

    public float[] Score { get; set; } = [];
}

/// <summary>
/// Trains a multiclass text-classification model (SDCA maximum entropy over featurized
/// n-gram text) on a small curated dataset of labeled claims, and exposes it for scoring
/// pasted claims at request time. Training runs once, in memory, when the singleton is
/// constructed at app startup — the dataset is small enough that this takes well under a
/// second and avoids needing to ship/load a separate model file.
/// </summary>
public class ClaimClassifierService
{
    private readonly MLContext _mlContext;
    private readonly PredictionEngine<ClaimTrainingExample, ClaimPrediction> _engine;
    private readonly string[] _classNames;

    public ClaimClassifierService()
    {
        _mlContext = new MLContext(seed: 42);

        var trainingData = SeedData.TrainingClaims
            .Select(t => new ClaimTrainingExample { Text = t.Text, Label = t.Label })
            .ToList();

        var dataView = _mlContext.Data.LoadFromEnumerable(trainingData);

        var pipeline = _mlContext.Transforms.Conversion
            .MapValueToKey(inputColumnName: "Label", outputColumnName: "Label")
            .Append(_mlContext.Transforms.Text.FeaturizeText(outputColumnName: "Features", inputColumnName: "Text"))
            .Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy(labelColumnName: "Label", featureColumnName: "Features"))
            .Append(_mlContext.Transforms.Conversion.MapKeyToValue(outputColumnName: "PredictedLabel", inputColumnName: "PredictedLabel"));

        var model = pipeline.Fit(dataView);

        _engine = _mlContext.Model.CreatePredictionEngine<ClaimTrainingExample, ClaimPrediction>(model);

        // Recover the ordered class names so Score[i] can be matched back to a label name.
        VBuffer<ReadOnlyMemory<char>> slotNames = default;
        _engine.OutputSchema["Score"].GetSlotNames(ref slotNames);
        _classNames = slotNames.DenseValues().Select(v => v.ToString()).ToArray();
    }

    public (string Label, float Confidence, Dictionary<string, float> ClassScores) Classify(string text)
    {
        var prediction = _engine.Predict(new ClaimTrainingExample { Text = text });

        var classScores = new Dictionary<string, float>();
        for (var i = 0; i < _classNames.Length && i < prediction.Score.Length; i++)
        {
            classScores[_classNames[i]] = prediction.Score[i];
        }

        var confidence = classScores.TryGetValue(prediction.PredictedLabel, out var score) ? score : 0f;

        return (prediction.PredictedLabel, confidence, classScores);
    }
}
