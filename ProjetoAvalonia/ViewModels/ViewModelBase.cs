using ReactiveUI; // ReactiveUI é uma biblioteca

namespace ProjetoAvalonia.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
    }
}

//A classe ViewModelBase serve como uma base comum para outras classes de ViewModels em um aplicativo que utiliza ReactiveUI.
//Ao herdar de ReactiveObject, a classe ViewModelBase ganha funcionalidades relacionadas à programação reativa,
//como a capacidade de ter propriedades reativas e notificar automaticamente a interface do usuário sobre mudanças nessas propriedades