namespace PetRego.Models
{
    /// Yes, it is nicer to work with an enum where possible. It means there is
    /// strongly-typed, compile-time-safe references and the code is clearer.
    /// Care needs to be taken when deciding this however, because new enum values
    /// require a code change, test and redeployment, all adding cost, time and 
    /// risk to the customer's desire to iterate on their product. 
    /// 
    /// I decided to go with an enum here because the brief stated;
    /// 'The type of animal (we particularly care about dogs, cats, chickens and snakes)'
    /// 
    /// In a real-world scenario, I would probably leave this as a string value
    /// and delegate management of values to the data layer.
    /// 
    /// 
    public enum PetType
    {
        Undefined,
        Dog,
        Cat,
        Chicken,
        Snake,
    }

}
