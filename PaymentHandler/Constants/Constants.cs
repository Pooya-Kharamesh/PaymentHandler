namespace PaymentHandler
{
    public static class Constants
    {
        // this address should be coming from a repository (config file or database or any other method) where the repository is injected to the class 
        public const string RoyaltyDepartmentAddress = "RoyaltyDepartmentAddress";

        public static class ItemNames
        {
            public const string MembershipUpgrade = "Membership Upgrade";
            public const string MembershipActivation = "Membership Activation";

            public const string LearnToSkiVideoName = "Learning to Ski";
            public const string FirstAidVideoName = "First Aid";
        }

        public static class StoreInformation
        {
            public const string NameAndAddress = "Awesome Store";
        }
    }
}
