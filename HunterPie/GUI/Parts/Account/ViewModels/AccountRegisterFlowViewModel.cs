﻿using HunterPie.Core.Client.Localization;
using HunterPie.Core.Notification;
using HunterPie.Integrations.Poogie.Account;
using HunterPie.Integrations.Poogie.Account.Models;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.UI.Architecture;
using System;

namespace HunterPie.GUI.Parts.Account.ViewModels;
public class AccountRegisterFlowViewModel : ViewModel
{
    private readonly PoogieAccountConnector _accountConnector = new();

    private string _username = string.Empty;
    private string _email = string.Empty;
    private string _password = string.Empty;
    private bool _canRegister;
    private bool _isRegistering;

    public string Username
    {
        get => _username;
        set
        {
            VerifyIfCanRegister();
            SetValue(ref _username, value);
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            VerifyIfCanRegister();
            SetValue(ref _email, value);
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            VerifyIfCanRegister();
            SetValue(ref _password, value);
        }
    }

    public bool CanRegister { get => _canRegister; set => SetValue(ref _canRegister, value); }

    public bool IsRegistering { get => _isRegistering; set => SetValue(ref _isRegistering, value); }

    public async void SignUp()
    {
        if (!CanRegister)
            return;

        IsRegistering = true;

        var request = new RegisterRequest(
            Username: Username,
            Email: Email,
            Password: Password
        );

        PoogieResult<RegisterResponse> register = await _accountConnector.Register(request);

        IsRegistering = false;

        if (register.Error is { } error)
        {
            NotificationService.Error(
                Localization.GetEnumString(error.Code),
                TimeSpan.FromSeconds(5)
            );

            return;
        }

        NotificationService.Success(
            Localization.QueryString("//Strings/Client/Integrations/Poogie[@Id='ACCOUNT_REGISTER_SUCCESS']")
                .Replace("{Email}", register.Response!.Email),
            TimeSpan.FromSeconds(10)
        );
    }

    private void VerifyIfCanRegister() => CanRegister = Username.Length > 0 && Email.Length > 0 && Password.Length > 0;
}
