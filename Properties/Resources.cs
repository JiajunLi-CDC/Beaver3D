using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Beaver3D.Properties
{
	// Token: 0x0200000A RID: 10
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		// Token: 0x06000078 RID: 120 RVA: 0x000044ED File Offset: 0x000026ED
		internal Resources()
		{
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000079 RID: 121 RVA: 0x000044F8 File Offset: 0x000026F8
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				bool flag = Resources.resourceMan == null;
				if (flag)
				{
					ResourceManager resourceManager = new ResourceManager("Beaver3D.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600007A RID: 122 RVA: 0x00004540 File Offset: 0x00002740
		// (set) Token: 0x0600007B RID: 123 RVA: 0x00004557 File Offset: 0x00002757
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00004560 File Offset: 0x00002760
		internal static Icon ConsoleIcon
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("ConsoleIcon", Resources.resourceCulture);
				return (Icon)@object;
			}
		}

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600007D RID: 125 RVA: 0x00004590 File Offset: 0x00002790
		internal static string HEASections
		{
			get
			{
				return Resources.ResourceManager.GetString("HEASections", Resources.resourceCulture);
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600007E RID: 126 RVA: 0x000045B8 File Offset: 0x000027B8
		internal static string IPESections
		{
			get
			{
				return Resources.ResourceManager.GetString("IPESections", Resources.resourceCulture);
			}
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600007F RID: 127 RVA: 0x000045E0 File Offset: 0x000027E0
		internal static string LSections
		{
			get
			{
				return Resources.ResourceManager.GetString("LSections", Resources.resourceCulture);
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000080 RID: 128 RVA: 0x00004608 File Offset: 0x00002808
		internal static string RHSections
		{
			get
			{
				return Resources.ResourceManager.GetString("RHSections", Resources.resourceCulture);
			}
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000081 RID: 129 RVA: 0x00004630 File Offset: 0x00002830
		internal static string SHSections
		{
			get
			{
				return Resources.ResourceManager.GetString("SHSections", Resources.resourceCulture);
			}
		}

		// Token: 0x0400002C RID: 44
		private static ResourceManager resourceMan;

		// Token: 0x0400002D RID: 45
		private static CultureInfo resourceCulture;
	}
}
