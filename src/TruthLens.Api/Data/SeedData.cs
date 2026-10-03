namespace TruthLens.Api.Data;

/// <summary>
/// Starter source-reputation directory, localized for Ghana. Ghanaian government and health
/// bodies, the national news agency and the main fact-checking initiatives are included.
/// The categories for individual Ghanaian news outlets are judgment calls and should be
/// reviewed by the team before submission. A few ".example" domains are kept to demonstrate
/// the "Mixed" and "LowReputation" categories without making a contestable real-world call
/// about a specific outlet — replace/extend this table with your own curated sources.
/// </summary>
public static class SeedData
{
    public static readonly SourceEntity[] Sources =
    [
        // --- Ghanaian news outlets ---
        new() { Domain = "graphic.com.gh", Name = "Graphic Online", Category = "HighReputation",
            Notes = "Long-running Ghanaian daily newspaper group with a national newsroom." },
        new() { Domain = "citinewsroom.com", Name = "Citi Newsroom", Category = "HighReputation",
            Notes = "Ghanaian digital news outlet with a national newsroom and regular fact-based reporting." },
        new() { Domain = "myjoyonline.com", Name = "MyJoyOnline (Joy News)", Category = "HighReputation",
            Notes = "Ghanaian broadcaster's online news platform; mainstream national coverage." },
        new() { Domain = "gna.org.gh", Name = "Ghana News Agency", Category = "Mixed",
            Notes = "State-owned national news agency. Factual wire-style reporting, but coverage can reflect official government positions — cross-check contested claims." },
        new() { Domain = "gbcghanaonline.com", Name = "Ghana Broadcasting Corporation (GBC)", Category = "Mixed",
            Notes = "State-owned public broadcaster. Reliable for official announcements; cross-check political reporting." },
        new() { Domain = "ghanaweb.com", Name = "GhanaWeb", Category = "Mixed",
            Notes = "High-traffic Ghanaian news aggregator and forum mixing wire copy, opinion and user comments — verify claims against the original source." },

        // --- Ghanaian government, health and electoral authorities ---
        new() { Domain = "ghs.gov.gh", Name = "Ghana Health Service", Category = "GovernmentOrHealthAuthority",
            Notes = "Ghana's main public health implementing agency; primary source for national health guidance and disease alerts." },
        new() { Domain = "moh.gov.gh", Name = "Ministry of Health, Ghana", Category = "GovernmentOrHealthAuthority",
            Notes = "Government ministry responsible for national health policy." },
        new() { Domain = "fda.gov.gh", Name = "Food and Drugs Authority, Ghana", Category = "GovernmentOrHealthAuthority",
            Notes = "National regulator for food, drugs and medical products; primary source on product safety and drug alerts." },
        new() { Domain = "ec.gov.gh", Name = "Electoral Commission of Ghana", Category = "GovernmentOrHealthAuthority",
            Notes = "Official body that conducts elections and publishes official results; primary source for election information." },
        new() { Domain = "statsghana.gov.gh", Name = "Ghana Statistical Service", Category = "GovernmentOrHealthAuthority",
            Notes = "Official national statistics agency; primary source for population and economic figures." },

        // --- Fact-checking initiatives ---
        new() { Domain = "dubawa.org", Name = "Dubawa", Category = "HighReputation",
            Notes = "West African fact-checking initiative that verifies viral claims with published methodology." },
        new() { Domain = "africacheck.org", Name = "Africa Check", Category = "HighReputation",
            Notes = "Pan-African nonprofit fact-checking organization." },

        // --- International references ---
        new() { Domain = "reuters.com", Name = "Reuters", Category = "HighReputation",
            Notes = "International wire service with a dedicated fact-checking desk." },
        new() { Domain = "who.int", Name = "World Health Organization", Category = "GovernmentOrHealthAuthority",
            Notes = "UN health authority; primary source for global health guidance." },
        new() { Domain = "nature.com", Name = "Nature", Category = "HighReputation",
            Notes = "Peer-reviewed scientific journal." },
        new() { Domain = "theonion.com", Name = "The Onion", Category = "Satire",
            Notes = "Well-known satirical publication — content is not factual reporting." },

        // --- Demo entries for the Mixed / LowReputation categories ---
        new() { Domain = "accra-daily-voice.example", Name = "Accra Daily Voice", Category = "Mixed",
            Notes = "Demo entry: blends factual reporting with strong editorial opinion — verify claims against a primary source." },
        new() { Domain = "clickburst-gh.example", Name = "ClickBurst Ghana", Category = "LowReputation",
            Notes = "Demo entry illustrating a low-credibility, engagement-driven content mill." },
        new() { Domain = "viral-health-tips.example", Name = "Viral Health Tips", Category = "LowReputation",
            Notes = "Demo entry illustrating an unverified health-advice content farm." },
    ];

    /// <summary>Labeled training examples for the ML.NET claim classifier.</summary>
    public static readonly (string Text, string Label)[] TrainingClaims =
    [
        // --- Credible: established facts / official guidance ---
        ("The World Health Organization recommends handwashing with soap to reduce the spread of infectious disease.", "Credible"),
        ("Vaccines undergo multiple phases of clinical trials before regulatory approval.", "Credible"),
        ("The Earth orbits the Sun once approximately every 365.25 days.", "Credible"),
        ("Regular exercise is associated with a lower risk of cardiovascular disease.", "Credible"),
        ("The United Nations General Assembly convenes annually in New York.", "Credible"),
        ("Mosquitoes can transmit malaria through their bites.", "Credible"),
        ("The CDC recommends annual flu vaccination for most adults.", "Credible"),
        ("Antibiotics are not effective against viral infections such as the common cold.", "Credible"),
        ("Election results in most countries are certified after an official vote count and audit process.", "Credible"),
        ("Smoking tobacco significantly increases the risk of lung cancer.", "Credible"),
        ("The human body is made up of roughly 60% water.", "Credible"),
        ("Polling places are required by law to be accessible to voters with disabilities in most democracies.", "Credible"),
        ("Climate scientists broadly agree that human activity is a major driver of recent global warming.", "Credible"),
        ("Public health officials recommend covering coughs and sneezes to limit disease spread.", "Credible"),
        ("Ballots in most jurisdictions are counted by election officials under bipartisan observation.", "Credible"),
        ("Drinking water helps the body regulate its temperature.", "Credible"),
        ("The measles vaccine has been shown in large studies to be safe and effective.", "Credible"),
        ("National election dates are set well in advance by law in most democracies.", "Credible"),
        ("Sunscreen reduces the risk of skin damage from UV radiation.", "Credible"),
        ("International election observers are often invited to monitor voting procedures for transparency.", "Credible"),
        ("Handwashing is one of the most effective ways to prevent the spread of germs.", "Credible"),
        ("The human immune system produces antibodies in response to vaccination.", "Credible"),
        ("Most countries require voters to register before an election.", "Credible"),
        ("Wearing a seatbelt reduces the risk of serious injury in a car crash.", "Credible"),
        ("Peer review is a standard part of publishing in reputable scientific journals.", "Credible"),

        // --- Uncertain: ambiguous, unverified, or opinion-flavored claims ---
        ("A new study suggests coffee might reduce the risk of a rare disease, but researchers say more research is needed.", "Uncertain"),
        ("Sources close to the campaign say the candidate may announce a policy change next week.", "Uncertain"),
        ("Some experts believe the new diet trend could have long-term health benefits, though evidence is limited.", "Uncertain"),
        ("An anonymous online post claims a celebrity was seen at a local restaurant last night.", "Uncertain"),
        ("A viral video appears to show unusual weather, but its location and date have not been verified.", "Uncertain"),
        ("Early exit polls suggest a close race, though official results are still being counted.", "Uncertain"),
        ("A blogger claims a household item can help with sleep, but no clinical trial has tested the claim.", "Uncertain"),
        ("Reports are circulating that a company may lay off staff, but the company has not confirmed this.", "Uncertain"),
        ("An unverified account on social media alleges irregularities at a polling site.", "Uncertain"),
        ("A preliminary, non-peer-reviewed study suggests a possible link between two conditions.", "Uncertain"),
        ("Some social media users claim a new supplement boosts memory, but no major study supports this yet.", "Uncertain"),
        ("A local news tip suggests a factory may be linked to pollution, pending official investigation.", "Uncertain"),
        ("Rumors are spreading online about a candidate's health, without any official statement.", "Uncertain"),
        ("An unconfirmed report claims a vote recount could change the local election outcome.", "Uncertain"),
        ("A viral post claims a natural remedy cures headaches, citing only personal testimonials.", "Uncertain"),
        ("Anonymous sources allege a policy change is being considered, but officials have not commented.", "Uncertain"),

        // --- Likely misinformation: well-documented false/debunked claims (health & election) ---
        ("5G cell towers cause COVID-19 infections.", "LikelyMisinformation"),
        ("Vaccines contain microchips used to track people.", "LikelyMisinformation"),
        ("Drinking bleach or industrial disinfectant cures viral infections.", "LikelyMisinformation"),
        ("The COVID-19 vaccine alters your DNA permanently.", "LikelyMisinformation"),
        ("Millions of dead people voted in a recent national election.", "LikelyMisinformation"),
        ("Voting machines secretly switch votes from one candidate to another without any evidence or audit trail.", "LikelyMisinformation"),
        ("A miracle herbal tea cures cancer within days.", "LikelyMisinformation"),
        ("Wearing a face mask causes dangerous oxygen deprivation in healthy adults.", "LikelyMisinformation"),
        ("Ballots were mass-shredded on election night with no investigation or evidence to support the claim.", "LikelyMisinformation"),
        ("Drinking colloidal silver cures all viral infections.", "LikelyMisinformation"),
        ("The moon landing was staged in a television studio.", "LikelyMisinformation"),
        ("A secret ingredient in tap water is being used to control the population.", "LikelyMisinformation"),
        ("Election officials nationwide were caught on camera burning valid ballots, according to a viral post with no verifiable source.", "LikelyMisinformation"),
        ("Essential oils can replace all vaccines and antibiotics.", "LikelyMisinformation"),
        ("A specific blood type makes a person immune to all viral disease.", "LikelyMisinformation"),
        ("Foreign governments directly altered vote totals in a national election with no evidence presented.", "LikelyMisinformation"),
        ("Microwaving food destroys all its nutrients and makes it toxic.", "LikelyMisinformation"),
        ("A common over-the-counter vitamin can cure any type of cancer.", "LikelyMisinformation"),
        ("Voter ID databases were secretly deleted to erase evidence of fraud, based on an unverified viral claim.", "LikelyMisinformation"),
        ("Chemtrails from airplanes are a deliberate government mind-control program.", "LikelyMisinformation"),
    ];
}
