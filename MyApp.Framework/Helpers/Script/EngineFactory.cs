using System;

namespace MyApp.Framework.Helpers.Script
{
    /// <summary>
    /// MyApp script engine factory.
    /// </summary>
    public static class EngineFactory
    {
        /// <summary>
        /// Gets a new MyApp script engine. The default engine is Jint.
        /// </summary>
        /// <param name="timeout">Script time limit.</param>
        /// <param name="memoryLimit">Maximum memory allocated for Jint engine.</param>
        /// <returns>MyApp script engine.</returns>
        public static IEngine GetEngine(TimeSpan timeout, long? memoryLimit) => new JintEngine(timeout, memoryLimit);
    }
}
