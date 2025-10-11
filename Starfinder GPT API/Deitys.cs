using GameData;

public class Deity
{
    public string Name {  get; set;}
    public string Description { get; set; }
    public Alignment Alignment { get; set; }

    public Deity(string name, string description, Alignment alignment)
    {
        Name = name;
        Description = description;
        Alignment = alignment;
    }
}

public static class Deities
{
    public static readonly Deity Hylax = new Deity("Hylax", "Goddess of diplomacy and peace, often revered by the Shirren.", Alignment.LawfulGood);
    public static readonly Deity Iomedae = new Deity("Iomedae", "Goddess of honor, justice, rulership, and valor.", Alignment.LawfulGood);
    public static readonly Deity Abadar = new Deity("Abadar", "God of civilization, law, and wealth.", Alignment.LawfulNeutral);
    public static readonly Deity Desna = new Deity("Desna", "Goddess of dreams, luck, stars, and travelers.", Alignment.ChaoticGood);
    public static readonly Deity Sarenrae = new Deity("Sarenrae", "Goddess of healing, redemption, and the sun.", Alignment.NeutralGood);
    public static readonly Deity Pharasma = new Deity("Pharasma", "Goddess of fate, death, and prophecy.",  Alignment.TrueNeutral);
    public static readonly Deity ZonKuthon = new Deity("Zon-Kuthon", "God of pain, darkness, and loss.",  Alignment.LawfulEvil);
    public static readonly Deity Asmodeus = new Deity("Asmodeus", "God of tyranny, slavery, and pride.",  Alignment.LawfulEvil);
    public static readonly Deity Calistria = new Deity("Calistria", "Goddess of lust, revenge, and trickery.",  Alignment.ChaoticNeutral);
    public static readonly Deity Lamashtu = new Deity("Lamashtu", "Goddess of madness, monsters, and nightmares.",  Alignment.ChaoticEvil);

    public static readonly Deity Yaraesa = new Deity("Yaraesa", "Goddess of knowledge, enlightenment, and sciences.",  Alignment.LawfulNeutral);
    public static readonly Deity Weydan = new Deity("Weydan", "God of change, trade, and freedom.",  Alignment.ChaoticGood);
    public static readonly Deity Talavet = new Deity("Talavet", "Goddess of community, storytelling, and communication.",  Alignment.NeutralGood);
    public static readonly Deity Eloritu = new Deity("Eloritu", "God of magic, mysteries, and secrets.",  Alignment.TrueNeutral);
    public static readonly Deity Ibra = new Deity("Ibra", "Goddess of cosmic phenomena and the unknown.",  Alignment.TrueNeutral);
    public static readonly Deity Triune = new Deity("Triune", "God of artificial intelligence, computers, and machines.",  Alignment.TrueNeutral);
    public static readonly Deity Besmara = new Deity("Besmara", "Goddess of piracy, space, and chaos.",  Alignment.ChaoticNeutral);
    public static readonly Deity Oras = new Deity("Oras", "God of evolution and natural processes.",  Alignment.TrueNeutral);
    public static readonly Deity Damoritosh = new Deity("Damoritosh", "God of war, conquest, and strength.",  Alignment.LawfulEvil);
    public static readonly Deity LaoShuPo = new Deity("Lao Shu Po", "Goddess of thievery, night, and rats.",  Alignment.NeutralEvil);
    public static readonly Deity Urgathoa = new Deity("Urgathoa", "Goddess of undeath, gluttony, and disease.",  Alignment.NeutralEvil);
    public static readonly Deity TheDevourer = new Deity("The Devourer", "God of destruction and nihilism.",  Alignment.ChaoticEvil);
    public static readonly Deity Nyarlathotep = new Deity("Nyarlathotep", "God of deception, chaos, and madness.",  Alignment.ChaoticEvil);
    public static readonly Deity Angradd = new Deity("Angradd", "God of fire, honor, and combat.",  Alignment.LawfulGood);
    public static readonly Deity Arshea = new Deity("Arshea", "Deity of freedom, beauty, and physical pleasure.",  Alignment.ChaoticGood);
    public static readonly Deity BlackButterfly = new Deity("Black Butterfly", "Goddess of silence, void, and space.",  Alignment.NeutralGood);
    public static readonly Deity Eldest = new Deity("Eldest", "Group of powerful fae beings representing the fae and nature.",  Alignment.TrueNeutral);
    public static readonly Deity Groetus = new Deity("Groetus", "God of end times and apocalypse.",  Alignment.NeutralEvil);
    public static readonly Deity Lissala = new Deity("Lissala", "Goddess of obedience, runes, and serpents.",  Alignment.LawfulEvil);
}