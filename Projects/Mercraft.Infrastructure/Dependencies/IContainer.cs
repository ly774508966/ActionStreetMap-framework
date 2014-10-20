﻿using System;
using System.Collections.Generic;
using Mercraft.Infrastructure.Dependencies.Interception.Behaviors;

namespace Mercraft.Infrastructure.Dependencies
{
    /// <summary>
    ///     Defines dependency injection container behavior.
    ///  </summary>
    public interface IContainer: IDisposable
    {
        #region Options

        /// <summary>
        ///     If set to false, proxy usage will be disabled.
        /// </summary>
        bool AllowProxy { get; set; }

        /// <summary>
        ///     If set to true than custom and autogenerated proxies are used for all classes if possible.
        /// </summary>
        bool AutoGenerateProxy { get; set; }

        #endregion

        #region Proxies

        /// <summary>
        ///     Adds global behavior.
        /// </summary>
        /// <param name="behavior">Behavior.</param>
        /// <returns>Container.</returns>
        IContainer AddGlobalBehavior(IBehavior behavior);

        #endregion

        #region Resolve single instance

        /// <summary>
        ///     Resolves object.
        /// </summary>
        /// <param name="type">Type.</param>
        /// <returns>Returns object.</returns>
        object Resolve(Type type);

        /// <summary>
        ///     Resolves object.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        object Resolve(Type type, string name);
        
        /// <summary>
        ///     Resolves object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Resolve<T>();

        /// <summary>
        ///     Resolves object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        T Resolve<T>(string name);

        #endregion

        #region Resolve several instances

        /// <summary>
        ///     Resolves enumerable of objects.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IEnumerable<T> ResolveAll<T>();

        /// <summary>
        ///     Resolves enumerable of objects.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<object> ResolveAll(Type type);

        #endregion

        #region Register component

        /// <summary>
        ///     Registers component.
        /// </summary>
        /// <param name="component">Component.</param>
        /// <returns>Container.</returns>
        IContainer Register(Component component);

        #endregion

        #region Register instance

        /// <summary>
        ///     Registers object.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="instance">Instance.</param>
        /// <returns>Container.</returns>
        IContainer RegisterInstance<T>(T instance);

        /// <summary>
        ///     Registers object.
        /// </summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="instance">Instance.</param>
        /// <param name="name">Name.</param>
        /// <returns>Container.</returns>
        IContainer RegisterInstance<T>(T instance, string name);

        /// <summary>
        ///     Registers object.
        /// </summary>
        /// <param name="t">Type.</param>
        /// <param name="instance">Instance.</param>
        /// <returns>Container.</returns>
        IContainer RegisterInstance(Type t, object instance);
        
        /// <summary>
        ///     Registers object.
        /// </summary>
        /// <param name="t">Type.</param>
        /// <param name="instance">Instance.</param>
        /// <param name="name">Name.</param>
        /// <returns>Container.</returns>
        IContainer RegisterInstance(Type t, object instance, string name);

        #endregion
    }
}
