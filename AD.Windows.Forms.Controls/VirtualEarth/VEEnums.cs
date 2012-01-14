namespace AD.Windows.Forms.Controls
{
    /// <summary>
    /// Enum of handling possibilities for disambiguatious search results
    /// </summary>
    public enum VEDisambiguation
    {
        Default = 1,
        Ignore = 0
    }

    /// <summary>
    /// Enum of available distinance units
    /// </summary>
    public enum VEDistanceUnit
    {
        Kilometers = 0,
        Miles = 1
    }

    /// <summary>
    /// Enum of available route modes
    /// </summary>
    public enum VERouteMode
    {
        Walking = 0,
        Driving = 1
    }

    /// <summary>
    /// Enum of available map styles
    /// </summary>
    public enum VEMapStyle
    {
        Road = 0,
        Aerial = 1,
        Birdseye = 2,
        Hybrid = 3
    }

    /// <summary>
    /// Enum of available dashboard sizes
    /// </summary>
    public enum VEDashboardStyle
    {
        Tiny = 0,
        Small = 1,
        Normal = 2
    }
}