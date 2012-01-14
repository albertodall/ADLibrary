using System;

namespace AD.Shared.Extensions
{
    public static class DisposableExtensions
    {
        /// <summary>
        /// Runs a delegate on the specified <see cref="IDisposable"/> and returns its value.
        /// </summary>
        /// <typeparam name="T">Type of the object implementing <see cref="IDisposable"/></typeparam>
        /// <typeparam name="TOut">The type of the return value.</typeparam>
        /// <param name="disposable">The object implementing <see cref="IDisposable"/>.</param>
        /// <param name="func">A delegate that takes <paramref name="disposable"/> as a parameter and returns a value.</param>
        /// <returns>Value returned by <paramref name="func"/> delegate.</returns>
        public static TOut Using<T, TOut>(this T disposable, Func<T, TOut> func) where T : IDisposable
        {
            using (disposable)
            {
                return func(disposable);
            }
        }

        /// <summary>
        /// Runs a delegate on the specified <see cref="IDisposable"/> and does not return anything.
        /// </summary>
        /// <typeparam name="T">Type of the object implementing <see cref="IDisposable"/></typeparam>
        /// <param name="disposable">The object implementing <see cref="IDisposable"/>.</param>
        /// <param name="action">A delegate that takes <paramref name="disposable"/> as a parameter.</param>
        public static void Using<T>(this T disposable, Action<T> action) where T : IDisposable
        {
            using (disposable)
            {
                action(disposable);
            }
        }
    }
}