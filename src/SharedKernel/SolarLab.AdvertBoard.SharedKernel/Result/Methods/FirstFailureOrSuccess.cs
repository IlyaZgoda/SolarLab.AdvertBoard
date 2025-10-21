namespace SolarLab.AdvertBoard.SharedKernel.Result
{
    public partial class Result
    {
        /// <summary>
        /// Возвращает первую неудачу из переданных результатов или успех, если все операции успешны.
        /// </summary>
        /// <param name="results">Массив результатов для проверки.</param>
        /// <returns>
        /// Первый неуспешный результат из переданных; 
        /// если все успешны - возвращает успешный результат.
        /// </returns>
        public static Result FirstFailureOrSuccess(params Result[] results)
        {
            foreach (Result result in results)
            {
                if (result.IsFailure)
                {
                    return result;
                }
            }

            return Success();
        }
    }
}
