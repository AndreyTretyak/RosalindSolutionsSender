using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RolalindSolver.Core.App_LocalResourses;
using RosalindSolver.Interfaces;

namespace RosalindSolver
{
    public class SendingManager
    {
        private readonly IUserInputProvider _inputProvider;
        private readonly IConfigurationProvider<UserConfiguration> _userConfigurationProvider;
        private readonly IConfigurationProvider<ServerConfiguration> _serverConfigurationProvider;
        private readonly ISelectedProblemProvider _selectedProblemProvider;
        private readonly IUnsolvedProblemProvider _unsolvedProblemProvider;
        private readonly ISolutionSender _sender;

        internal class MenuItem
        {
            public string Name { get; }

            public Func<Task> Action { get; }

            public MenuItem(string name, Func<Task> action)
            {
                Name = name;
                Action = action;
            }
        }

        private readonly MenuItem[] _settingMenu; 

        public SendingManager(
            IUserInputProvider inputProvider,
            IConfigurationProvider<UserConfiguration> userConfigurationProvider,
            IConfigurationProvider<ServerConfiguration> serverConfigurationProvider,
            ISelectedProblemProvider selectedProblemProvider,
            IUnsolvedProblemProvider unsolvedProblemProvider,
            ISolutionSender sender)
        {
            _inputProvider = inputProvider;
            _userConfigurationProvider = userConfigurationProvider;
            _serverConfigurationProvider = serverConfigurationProvider;
            _selectedProblemProvider = selectedProblemProvider;
            _unsolvedProblemProvider = unsolvedProblemProvider;
            _sender = sender;

            _settingMenu = new []
            {
                new MenuItem(Resources.SendCurrentlySelected, ContinueAsync),
                new MenuItem(Resources.ChangeCurrentProblem, ChangeCurrentProblemAsync),
                new MenuItem(Resources.ChangeUserSettings, ChangeUserSettingsAsync),
                new MenuItem(Resources.ChangeServerSettings, ChangeServerSettingsAsync),
                new MenuItem(Resources.SendAllUnsolved, SendAllUnsolvedAsync),
                new MenuItem(Resources.ClearSolvedProblemsRecords, ClearSolvedProblemsRecordsAsync),
                new MenuItem(Resources.SendAllSolutions, SendAllSolutionsAsync)
            };
        }

        public async Task StartExecutionLoopAsync()
        {
            while (true)
            {
                var item = _inputProvider.SelectOption(_settingMenu, e => e.Name);
                await item.Action();
            }
        }

        private async Task ContinueAsync()
        {
            var key = _selectedProblemProvider.GetCurrentProblemKey();
            await _sender.SendAsync(key);
        }

        private Task ChangeCurrentProblemAsync()
        {
            _selectedProblemProvider.ClearSelected();
            return Task.CompletedTask;
        }

        private Task ChangeUserSettingsAsync()
        {
            _userConfigurationProvider.ClearConfiguration();
            return Task.CompletedTask;
        }

        private Task ChangeServerSettingsAsync()
        {
            _serverConfigurationProvider.ClearConfiguration();
            return Task.CompletedTask;
        }

        private Task ClearSolvedProblemsRecordsAsync()
        {
            _unsolvedProblemProvider.ClearSolvedMarks();
            return Task.CompletedTask;
        }

        private Task SendAllUnsolvedAsync()
        {
            return SendSolutionsAsync(_unsolvedProblemProvider.GetUnsolvedProblems());
        }

        private Task SendAllSolutionsAsync()
        {
            return SendSolutionsAsync(_selectedProblemProvider.AvailableSolvers());
        }

        private async Task SendSolutionsAsync(IEnumerable<string> keys)
        {
            await Task.WhenAll(keys.Select(SendSolutionAsync));
        }

        private async Task SendSolutionAsync(string key)
        {
            var result = await _sender.SendAsync(key);
            if (result.IsCorrect) _unsolvedProblemProvider.MarkAsSolved(key);
        }
    }
}