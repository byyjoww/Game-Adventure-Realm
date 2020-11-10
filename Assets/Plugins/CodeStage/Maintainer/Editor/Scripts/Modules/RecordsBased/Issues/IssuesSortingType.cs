﻿#region copyright
//------------------------------------------------------------------------
// Copyright (C) Dmitriy Yukhanov - focus [http://codestage.net]
//------------------------------------------------------------------------
#endregion

namespace CodeStage.Maintainer.Cleaner
{
	internal enum IssuesSortingType : byte
	{
		Unsorted,
		ByIssueType,
		BySeverity,
		ByPath
	}
}