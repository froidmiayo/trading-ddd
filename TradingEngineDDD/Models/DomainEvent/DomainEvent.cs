using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TradingEngineDDD.Models.Entity;
using TradingEngineDDD.Models.ValueObject;

namespace TradingEngineDDD.Models.DomainEvent
{
    public abstract class DomainEvent
    {
        protected DomainEvent()
        {
            EventId = new Guid();
            EventTimeStamp = DateTime.Now;
        }
        public Guid EventId { get; }
        public DateTime EventTimeStamp { get; }
    }

    public class CurrencyExchangeSuccessfulEvent : DomainEvent
    {
        public CurrencyExchangeSuccessfulEvent(ClientId clientId, Currency currencyFrom, Currency currencyTo,
            AccountMoney amount, CurrencyConversion conversion)
        {
            ClientId = clientId.Value;
            CurrencyFrom = currencyFrom.Code;
            CurrencyTo = currencyTo.Code;
            Amount = amount.Value;
            Conversion = conversion.ConversionRate.Value;
        }
        public int ClientId { get; }
        public string CurrencyFrom { get; }
        public string CurrencyTo { get; }
        public decimal Amount { get; }
        public  decimal Conversion { get; }
    }

    public class FundTransferSuccessfulEvent : DomainEvent
    {
        public FundTransferSuccessfulEvent(int senderId, int recipientId, decimal amount, Currency currency)
        {
            SenderId = senderId;
            RecipientId = recipientId;
            Amount = amount;
            Currency = currency.Code;
        }
        public int SenderId { get; }
        public int RecipientId { get; }
        public string Currency { get; }
        public decimal Amount { get; }

    }
}