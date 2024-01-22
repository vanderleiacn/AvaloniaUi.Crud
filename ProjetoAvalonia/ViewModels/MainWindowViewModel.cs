


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
    public class MainWindowViewModel : ViewModelBase //Herdada
    {
#pragma warning disable CA1822 // Mark members as static
        public string Greeting => "Cadastro de Clientes";//propriedade apenas para leitura, valor declarado direto na propriedade
#pragma warning restore CA1822 // Mark members as static

        // ICommand sao as propriedades que representao o comando. Os comandos são geralmente usados para associar ações a elementos da UI
        public ObservableCollection<Cliente> Clientes { get; } = new ObservableCollection<Cliente>();//propriedade que representa uma COLEÇÃO observável de objetos do tipo Cliente ela notifica automaticamente a UI quando os itens sao adicionados,removidos...
        public ICommand SalvarEventCommand { get; }
        public ICommand BuscarEventCommand { get; }
        public ICommand ExcluirEventCommand { get; }
        public ICommand LimparEventCommand { get; }

        private string? _NomeCliente;//Campos privados
        public string Id { get; set; }
        public string? NomeCliente
        {
            get => _NomeCliente;//São propriedades públicas associadas aos campos privados. RaiseAndSetIfChanged indica que a notificação de mudança é feita automaticamente quando o valor é modificado.
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

        public MainWindowViewModel()// O construtor realiza as inicializações dos comandos e a coleçao de Clientes
        {
            this.Id = null;
            SalvarEventCommand = ReactiveCommand.Create(SalvarEventAsync);
            BuscarEventCommand = ReactiveCommand.Create(BuscarEventAsync);
            ExcluirEventCommand = ReactiveCommand.Create(ExcluirEventAsync);
            LimparEventCommand = ReactiveCommand.Create(LimparEvent);

            Clientes = new ObservableCollection<Cliente>();
            //A implementação faz uso de ReactiveUI para facilitar a manipulação de propriedades e eventos assíncronos.
        }

        //relacionado à lógica de eventos associados à manipulação de clientes
        public async Task SalvarEventAsync()
        {
            if (string.IsNullOrEmpty(this.NomeCliente) || string.IsNullOrEmpty(this.EmailCliente) || string.IsNullOrEmpty(this.TelefoneCliente))//verifica se as informaçoes obrigatorias do cliente estao preenchidas. se nao estiver retorna uma msg de erro
            {
                await ShowMessage("Favor preencher todos os dados do cliente!", ButtonEnum.Ok);
                return;
            }

            if (this.Clientes.FirstOrDefault(x => x.Nome == this.NomeCliente && x.Id != this.Id) != null)//Verifica se o cliente já está cadastrado na lista com base no nome,se sim, exibe uma msg informando que o cliente já está cadastrado.
            {
                await ShowMessage("Cliente ja cadastrado!", ButtonEnum.Ok);
                return;
            }

            var msg = "Novo cliente adicionado com sucesso!";

            if (this.Id != null)
            {
                var cliente = this.Clientes.FirstOrDefault(x => x.Id == this.Id);//Se o cliente já existir (com base no Id), remove o cliente antigo da lista.
                this.Clientes.Remove(cliente);

                msg = "Cliente atualizado com sucesso!";
            }

            this.Clientes.Add(new Cliente //Adiciona um novo cliente à lista ou atualiza o cliente existente
            {
                Id = this.Id ?? Guid.NewGuid().ToString(),
                Nome = this.NomeCliente,
                Email = this.EmailCliente,
                Telefone = this.TelefoneCliente
            });

            this.LimparEvent();//Limpa os campos e exibe uma mensagem indicando sucesso

            await ShowMessage(msg, ButtonEnum.Ok);
        }

        public async Task BuscarEventAsync()
        {
            var searchResult = this.Clientes.FirstOrDefault(x => x.Nome == this.NomeCliente);//Procura um cliente na lista com base no nome, se for encontrado preenche os campos com os campos e se nao for encontrado retorna a msg

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
            if (this.Id == null)//Verifica se um cliente foi previamente buscado (tem um Id associado), se nao foi buscado retorna a msg
            {
                await ShowMessage("É necessário buscar o cliente, antes de excluir!", ButtonEnum.Ok);
                return;
            }

            var cliente = this.Clientes.FirstOrDefault(x => x.Id == this.Id);//Remove o cliente correspondente da lista
            this.Clientes.Remove(cliente);

            this.LimparEvent();//Limpa os campos e exibe uma msg indicando sucesso
            await ShowMessage("Cliente excluído com sucesso!", ButtonEnum.Ok);
        }

        private static async Task ShowMessage(string message, ButtonEnum buttonType)// NAO ENTENDI
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Cadastro", message, buttonType)
                .ShowAsync();
        }

        public void LimparEvent()//Limpa os campos relacionados ao cliente
        {
            this.Id = null;
            this.NomeCliente = string.Empty;
            this.EmailCliente = string.Empty;
            this.TelefoneCliente = string.Empty;
        }
    }
}