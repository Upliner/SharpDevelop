﻿// Copyright (c) AlphaSierraPapa for the SharpDevelop Team (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using ICSharpCode.Core;
using ICSharpCode.SharpDevelop.Gui;
using ICSharpCode.SharpDevelop.Internal.Templates;
using ICSharpCode.SharpDevelop.Project;
using ICSharpCode.SharpDevelop.Project.Commands;
using ICSharpCode.SharpDevelop.Project.Dialogs;
using ICSharpCode.SharpDevelop.Templates;

namespace ICSharpCode.SharpDevelop
{
	class UIService : IUIService
	{
		public void ShowSolutionConfigurationEditorDialog(ISolution solution)
		{
			using (SolutionConfigurationEditor sce = new SolutionConfigurationEditor(solution)) {
				sce.ShowDialog(SD.WinForms.MainWin32Window);
				if (solution.IsDirty)
					solution.Save();
				foreach (IProject project in solution.Projects) {
					project.Save();
				}
			}
		}
		
		public FileTemplateResult ShowNewFileDialog(IProject project, DirectoryName directory, IEnumerable<FileTemplate> templates)
		{
			using (NewFileDialog nfd = new NewFileDialog(project, directory, templates ?? SD.Templates.FileTemplates)) {
				if (nfd.ShowDialog(SD.WinForms.MainWin32Window) == DialogResult.OK)
					return nfd.result;
				else
					return null;
			}
		}
		
		public ProjectTemplateResult ShowNewProjectDialog(ISolutionFolder solutionFolder, IEnumerable<ProjectTemplate> templates)
		{
			using (NewProjectDialog npdlg = new NewProjectDialog(createNewSolution: solutionFolder == null, projectTemplates: templates ?? SD.Templates.ProjectTemplates)) {
				npdlg.SolutionFolder = solutionFolder;
				if (solutionFolder != null) {
					npdlg.InitialProjectLocationDirectory = AddNewProjectToSolution.GetInitialDirectorySuggestion(solutionFolder);
				}
				
				// show the dialog to request project type and name
				if (npdlg.ShowDialog(SD.WinForms.MainWin32Window) == DialogResult.OK) {
					return npdlg.result;
				} else {
					return null;
				}
			}
		}
	}
}
