


using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ProjetoAvalonia.Models;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjetoAvalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
#pragma warning disable CA1822 // Mark members as static
        public string Greeting => "Cadastro de Clientes";
#pragma warning restore CA1822 // Mark members as static

        public ObservableCollection<Cliente> Clientes { get; } = new ObservableCollection<Cliente>();
        public ICommand SalvarEventCommand { get; }
        public ICommand BuscarEventCommand { get; }
        public ICommand ExcluirEventCommand { get; }
        public ICommand LimparEventCommand { get; }

        private string? _NomeCliente;
        public string Id { get; set; }
        public string? NomeCliente
        {
            get => _NomeCliente;
            set => this.RaiseAndSetIfChanged(ref _NomeCliente, value);
        }
        private string? _EmailCliente;
        public string? EmailCliente
        {
            get => _EmailCliente;
            set => this.RaiseAndSetIfChanged(ref _EmailCliente, value);
        }
        private string? _TelefoneCliente;
        public string? TelefoneCliente
        {
            get => _TelefoneCliente;
            set => this.RaiseAndSetIfChanged(ref _TelefoneCliente, value);
        }

        public MainWindowViewModel()
        {
            this.Id = null;
            SalvarEventCommand = ReactiveCommand.Create(SalvarEventAsync);
            BuscarEventCommand = ReactiveCommand.Create(BuscarEventAsync);
            ExcluirEventCommand = ReactiveCommand.Create(ExcluirEventAsync);
            LimparEventCommand = ReactiveCommand.Create(LimparEvent);

            Clientes = new ObservableCollection<Cliente>();
        }


        public async Task SalvarEventAsync()
        {
            if (string.IsNullOrEmpty(this.NomeCliente) || string.IsNullOrEmpty(this.EmailCliente) || string.IsNullOrEmpty(this.TelefoneCliente))
            {
                await ShowMessage("Favor preencher todos os dados do cliente!", ButtonEnum.Ok);
                return;
            }

            if (this.Clientes.FirstOrDefault(x => x.Nome == this.NomeCliente && x.Id != this.Id) != null)
            {
                await ShowMessage("Cliente ja cadastrado!", ButtonEnum.Ok);
                return;
            }

            var msg = "Novo cliente adicionado com sucesso!";

            if (this.Id != null)
            {
                var cliente = this.Clientes.FirstOrDefault(x => x.Id == this.Id);
                this.Clientes.Remove(cliente);

                msg = "Cliente atualizado com sucesso!";
            }

            this.Clientes.Add(new Cliente
            {
                Id = this.Id ?? Guid.NewGuid().ToString(),
                Nome = this.NomeCliente,
                Email = this.EmailCliente,
                Telefone = this.TelefoneCliente
            });

            this.LimparEvent();

            await ShowMessage(msg, ButtonEnum.Ok);
        }

        public async Task BuscarEventAsync()
        {
            var searchResult = this.Clientes.FirstOrDefault(x => x.Nome == this.NomeCliente);

            if (searchResult == null)
            {
                await ShowMessage("Cliente não encontrado!", ButtonEnum.Ok);
                return;
            }

            this.Id = searchResult.Id;
            this.NomeCliente = searchResult.Nome;
            this.EmailCliente = searchResult.Email;
            this.TelefoneCliente = searchResult.Telefone;
        }

        public async Task ExcluirEventAsync()
        {
            if (this.Id == null)
            {
                await ShowMessage("É necessário buscar o cliente, antes de excluir!", ButtonEnum.Ok);
                return;
            }

            var cliente = this.Clientes.FirstOrDefault(x => x.Id == this.Id);
            this.Clientes.Remove(cliente);

            this.LimparEvent();
            await ShowMessage("Cliente excluído com sucesso!", ButtonEnum.Ok);
        }

        private static async Task ShowMessage(string message, ButtonEnum buttonType)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Cadastro", message, buttonType)
                .ShowAsync();
        }

        public void LimparEvent()
        {
            this.Id = null;
            this.NomeCliente = string.Empty;
            this.EmailCliente = string.Empty;
            this.TelefoneCliente = string.Empty;
        }
    }
}