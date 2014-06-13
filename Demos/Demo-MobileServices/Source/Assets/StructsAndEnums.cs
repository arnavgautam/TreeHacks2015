using System;

namespace ADALBinding
{
	public enum ADAuthenticationResultStatus
	{
		AD_SUCCEEDED,
		AD_USER_CANCELLED,
		AD_FAILED
	}

	public enum ADPromptBehavior
	{
		AD_PROMPT_AUTO,
		AD_PROMPT_NEVER,
		AD_PROMPT_ALWAYS
	}
}

