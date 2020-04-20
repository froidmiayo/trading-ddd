using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TradingEngineDDD.Models.ValueObject;

namespace TradingEngineDDD.Models.Entity
{
    public class FundTransferRequest
    {
        public FundTransferRequest(List<Account> senderAccounts, List<Account> recipientAccounts,
            FundTransferRequestUnit fundTransferDetails)
        {
            if (senderAccounts == null || senderAccounts.Count == 0)
                throw new ArgumentException(nameof(senderAccounts));
            if (fundTransferDetails == null)
                throw new ArgumentException(nameof(fundTransferDetails));
            if(senderAccounts.All(x => x.Currency.Code != fundTransferDetails.Currency.Code))
                throw new ArgumentOutOfRangeException(nameof(fundTransferDetails),"fundTransfer currency should exist on sender's account");

            var fromAccount = senderAccounts.First(x => x.Currency.Code == fundTransferDetails.Currency.Code);
            var toAccount = recipientAccounts?.FirstOrDefault(x => x.Currency.Code == fundTransferDetails.Currency.Code);

            ModifiedAccounts = new List<Account>
            {
                new Account(new Currency(fromAccount.Currency.Code), new AccountId(fromAccount.AccountId.Value),
                    fromAccount.Balance.Deduct(fundTransferDetails.Amount.Value), new ClientId(fromAccount.ClientId.Value)),
                toAccount == null
                    ? new Account(new Currency(fundTransferDetails.Currency.Code), new AccountId(0),
                        new AccountMoney(fundTransferDetails.Amount.Value), new ClientId(fundTransferDetails.RecipientClientId.Value))
                    : new Account(new Currency(toAccount.Currency.Code), new AccountId(toAccount.AccountId.Value),
                        toAccount.Balance.Add(fundTransferDetails.Amount.Value), new ClientId(toAccount.ClientId.Value))
            };
        }

        public List<Account> ModifiedAccounts { get; }
    }
}