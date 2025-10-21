namespace SolarLab.AdvertBoard.SharedKernel
{
    /// <summary>
    /// Абстрактный record для строго типизированных идентификаторов.
    /// </summary>
    public abstract record StronglyTypedId(Guid Id)
    {
        /// <summary>
        /// Неявное преобразование строго типизированного идентификатора в Guid.
        /// </summary>
        /// <param name="Value">Строго типизированный идентификатор.</param>
        /// <returns>Значение Guid идентификатора.</returns>
        public static implicit operator Guid(StronglyTypedId Value) => Value.Id;
    }
}
