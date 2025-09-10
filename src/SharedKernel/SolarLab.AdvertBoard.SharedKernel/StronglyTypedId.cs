namespace SolarLab.AdvertBoard.SharedKernel
{
    public abstract record StronglyTypedId(Guid Id)
    {
        public static implicit operator Guid(StronglyTypedId Value) => Value.Id;
    }
}
