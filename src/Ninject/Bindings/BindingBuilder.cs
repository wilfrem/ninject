﻿using System;
using System.Threading;
using System.Web;
using Ninject.Activation;
using Ninject.Creation;
using Ninject.Syntax;

namespace Ninject.Bindings
{
	public class BindingBuilder : IBindingToSyntax, IBindingWhenOrInScopeSyntax, IBindingMetadataWhenOrInScopeSyntax, IBindingWithNameMetadataWhenOrInScopeSyntax
	{
		public Binding Binding { get; set; }

		public BindingBuilder(Binding binding)
		{
			Binding = binding;
		}

		public IBindingWithNameMetadataWhenOrInScopeSyntax ToSelf()
		{
			Binding.ProviderCallback = StandardProvider.GetCreationCallback(Binding.Service);
			return this;
		}

		public IBindingWithNameMetadataWhenOrInScopeSyntax To<TImplementation>()
		{
			Binding.ProviderCallback = StandardProvider.GetCreationCallback(typeof(TImplementation));
			return this;
		}

		public IBindingWithNameMetadataWhenOrInScopeSyntax To(Type implementation)
		{
			Binding.ProviderCallback = StandardProvider.GetCreationCallback(implementation);
			return this;
		}

		public IBindingWithNameMetadataWhenOrInScopeSyntax ToMethod<T>(Func<IContext, T> method)
		{
			Binding.ProviderCallback = ctx => new CallbackProvider<T>(method);
			return this;
		}

		public IBindingWithNameMetadataWhenOrInScopeSyntax ToProvider<TProvider>()
			where TProvider : IProvider
		{
			Binding.ProviderCallback = ctx => ctx.Kernel.Get<TProvider>();
			return this;
		}

		public IBindingWithNameMetadataWhenOrInScopeSyntax ToProvider(IProvider provider)
		{
			Binding.ProviderCallback = ctx => provider;
			return this;
		}

		public IBindingWithNameMetadataWhenOrInScopeSyntax ToConstant<T>(T value)
		{
			Binding.ProviderCallback = ctx => new ConstantProvider<T>(value);
			return this;
		}

		public IBindingMetadataWhenOrInScopeSyntax WithName(string name)
		{
			String.Intern(name);
			Binding.Metadata.Name = name;
			return this;
		}

		public IBindingMetadataWhenOrInScopeSyntax WithMetadata(string key, object value)
		{
			Binding.Metadata.Set(key, value);
			return this;
		}

		public IBindingInScopeSyntax When(Func<IRequest, bool> condition)
		{
			Binding.ConditionCallback = condition;
			return this;
		}

		public void InSingletonScope()
		{
			Binding.ScopeCallback = ctx => ctx.Kernel;
		}

		public void InTransientScope()
		{
			Binding.ScopeCallback = null;
		}

		public void InThreadScope()
		{
			Binding.ScopeCallback = ctx => Thread.CurrentThread;
		}

		public void InRequestScope()
		{
			Binding.ScopeCallback = ctx => HttpContext.Current.Request;
		}

		public void InScope(Func<IContext, object> scope)
		{
			Binding.ScopeCallback = scope;
		}
	}
}