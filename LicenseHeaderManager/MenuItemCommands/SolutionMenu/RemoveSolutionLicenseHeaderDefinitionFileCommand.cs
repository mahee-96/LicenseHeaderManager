﻿using System;
using System.ComponentModel.Design;
using System.IO;
using EnvDTE;
using LicenseHeaderManager.Headers;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace LicenseHeaderManager.MenuItemCommands.SolutionMenu
{
  /// <summary>
  ///   Command handler
  /// </summary>
  internal sealed class RemoveSolutionLicenseHeaderDefinitionFileCommand
  {
    /// <summary>
    ///   Command ID.
    /// </summary>
    public const int CommandId = 4134;

    /// <summary>
    ///   Command menu group (command set GUID).
    /// </summary>
    public static readonly Guid CommandSet = new Guid ("1a75d6da-3b30-4ec9-81ae-72b8b7eba1a0");

    private readonly OleMenuCommand _menuItem;

    /// <summary>
    ///   Initializes a new instance of the <see cref="RemoveSolutionLicenseHeaderDefinitionFileCommand" /> class.
    ///   Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    private RemoveSolutionLicenseHeaderDefinitionFileCommand (AsyncPackage package, OleMenuCommandService commandService)
    {
      ServiceProvider = (LicenseHeadersPackage) package ?? throw new ArgumentNullException (nameof(package));
      commandService = commandService ?? throw new ArgumentNullException (nameof(commandService));

      var menuCommandID = new CommandID (CommandSet, CommandId);
      _menuItem = new OleMenuCommand (Execute, menuCommandID);
      _menuItem.BeforeQueryStatus += OnQuerySolutionCommandStatus;
      commandService.AddCommand (_menuItem);
    }

    /// <summary>
    ///   Gets the instance of the command.
    /// </summary>
    public static RemoveSolutionLicenseHeaderDefinitionFileCommand Instance { get; private set; }

    /// <summary>
    ///   Gets the service provider from the owner package.
    /// </summary>
    private LicenseHeadersPackage ServiceProvider { get; }

    private void OnQuerySolutionCommandStatus (object sender, EventArgs e)
    {
      _menuItem.Enabled = ServiceProvider.SolutionHeaderDefinitionExists();
    }

    /// <summary>
    ///   Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync (AsyncPackage package)
    {
      // Switch to the main thread - the call to AddCommand in RemoveSolutionLicenseHeaderDefinitionFileCommand's constructor requires
      // the UI thread.
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync (package.DisposalToken);

      var commandService = await package.GetServiceAsync (typeof (IMenuCommandService)) as OleMenuCommandService;
      Instance = new RemoveSolutionLicenseHeaderDefinitionFileCommand (package, commandService);
    }

    /// <summary>
    ///   This function is the callback used to execute the command when the menu item is clicked.
    ///   See the constructor to see how the menu item is associated with this function using
    ///   OleMenuCommandService service and MenuCommand class.
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="e">Event args.</param>
    private void Execute (object sender, EventArgs e)
    {
      ThreadHelper.ThrowIfNotOnUIThread();

      var solutionHeaderDefinitionFilePath = LicenseHeader.GetHeaderDefinitionFilePathForSolution (ServiceProvider.Dte2.Solution);

      // Look for and close the document representing the license header definition file if it exists
      foreach (Document document in ServiceProvider.Dte2.Solution.DTE.Documents)
      {
        if (!string.Equals (solutionHeaderDefinitionFilePath, document.FullName, StringComparison.OrdinalIgnoreCase))
          continue;

        document.Close();
      }

      if (File.Exists (solutionHeaderDefinitionFilePath))
        File.Delete (solutionHeaderDefinitionFilePath);
    }
  }
}