using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.ViewModels;
using Microsoft.Maui.Controls;

namespace Mde.Project.Mobile.Pages;

public partial class TranslatePage : ContentPage{
    private readonly ISpeechService _speechService;
    private readonly ITranslationService _translationService;
    private readonly ITranslationStorageService _translationStorageService;
   
    public TranslatePage(TranslateViewModel translateViewModel, ISpeechService speechService, ITranslationService translationService,
        ITranslationStorageService translationStorageService){
        _speechService = speechService;
        _translationService = translationService;
        _translationStorageService = translationStorageService;
        InitializeComponent();
    BindingContext = new TranslateViewModel(_speechService, _translationService, _translationStorageService);
    }
}