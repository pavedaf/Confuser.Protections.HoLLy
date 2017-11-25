﻿using System;
using System.Collections.Generic;
using System.Linq;
using Confuser.Core;
using Confuser.Core.Helpers;
using Confuser.Core.Services;
using Confuser.Protections.HoLLy.FakeObuscator.Types;
using dnlib.DotNet;

namespace Confuser.Protections.HoLLy.FakeObuscator
{
    public class FakeObfuscatorPhase : ProtectionPhase
    {
        private const string DefaultNamespace = "Confuser.Protections.HoLLy.FakeObuscator";

        public override ProtectionTargets Targets => ProtectionTargets.Modules;
        public override string Name => "Fake obfuscator type addition";

        public FakeObfuscatorPhase(ConfuserComponent parent) : base(parent) { }

        protected override void Execute(ConfuserContext context, ProtectionParameters parameters)
        {
			//inject types
            foreach (ModuleDef m in parameters.Targets.OfType<ModuleDef>()) {
                InjectType(m, context.Logger, AgileDotNet.GetTypeDefs());	//+10

                InjectType(m, context.Logger, BabelDotNet.GetTypeDefs());	//+10
                InjectType(m, context.Logger, BabelDotNet.GetTypes());		//+110

                InjectType(m, context.Logger, CodeFort.GetTypes());         //+100

                InjectType(m, context.Logger, CodeWall.GetTypes());         //+100

                InjectType(m, context.Logger, CryptoObfuscator.GetTypeDefs()); //+10
                InjectType(m, context.Logger, CryptoObfuscator.GetTypes()); //+120

                InjectType(m, context.Logger, Dotfuscator.GetTypeDefs());   //+10
	            InjectType(m, context.Logger, Dotfuscator.GetTypes());      //+100

	            InjectType(m, context.Logger, EazfuscatorDotNet.GetTypes()); //+100

				InjectType(m, context.Logger, GoliathDotNet.GetTypeDefs()); //+10
                InjectType(m, context.Logger, GoliathDotNet.GetTypes());    //+100

	            InjectType(m, context.Logger, SpicesDotNet.GetTypeDefs()); //+10

				InjectType(m, context.Logger, Xenocode.GetTypeDefs());      //+10
	            InjectType(m, context.Logger, Xenocode.GetTypes());         //+100

	            InjectType(m, context.Logger, SmartAssembly.GetTypeDefs()); //+10


				//in case unknown obfuscator is forced
				InjectType(m, context.Logger, new TypeDefUser("YanoAttribute"));
            }

			//TODO: obfuscate names in DefaultNamespace
		}

		private static void InjectType(ModuleDef m, Core.ILogger l, params Type[] types)
	    {
		    foreach (TypeDef type in types.Select(RuntimeHelper.GetType)) {
			    var newType = new TypeDefUser(string.IsNullOrEmpty(type.Namespace) || type.Namespace.StartsWith("ConfuserEx")
				    ? DefaultNamespace
				    : type.Namespace.ToString(), type.Name);
			    m.Types.Add(newType);
			    l.Debug("Added type " + newType);

			    InjectHelper.Inject(type, newType, m);
		    }
		}

		private static void InjectType(ModuleDef m, Core.ILogger l, params TypeDef[] types)
		{
			foreach (TypeDef type in types) {
				m.Types.Add(type);
				l.Debug("Added type " + type);

				InjectHelper.Inject(type, type, m);
			}
		}
    }
}
