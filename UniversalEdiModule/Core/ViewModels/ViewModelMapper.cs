namespace UniversalEdiModule.Core.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DomainEntities;

    public static class ViewModelMapper
    {
        public static WaybillViewModel GetWaybillViewModel(Waybill waybill)
        {
            if(waybill == null)
                return null;

            return new WaybillViewModel
            {
              //  Buyer = waybill.Header.Buyer.Наименование,
                Date = waybill.Date,
                DeliveryPlace = waybill.Header.DeliveryPlace.Наименование,
                DownloadDate = waybill.DownloadDate,
                ID = waybill.ID,
                Number = waybill.Number,
                Supplier = waybill.Header.Supplier.Наименование,
                Positions = ViewModelMapper.GetWarePositionViewModels(waybill.Header.Positions)
            };
        }

        public static WarePositionViewModel GetWarePositionViewModel(WarePosition position)
        {
            if (position == null)
                return null;

            return new WarePositionViewModel
            {
                Amount = position.Amount,
                AmountWithVat = position.AmountWithVat,
                Barcode = position.Barcode,
                Number = position.Number,
                Price = position.Price,
                Quantity = position.Quantity,
                TaxRate = position.TaxRate,
                Unit = position.Unit,
                WareName = position.WareName,
                WareSupplierCode = position.WareSupplierCode
            };
        }

        public static List<WarePositionViewModel> GetWarePositionViewModels(List<WarePosition> positions)
        {
            if (positions == null)
                return null;

            List<WarePositionViewModel> result = new List<WarePositionViewModel>();

            foreach (var item in positions)
            {
                result.Add(ViewModelMapper.GetWarePositionViewModel(item));
            }

            return result;
        }

        public static List<WaybillViewModel> GetWaybillViewModels(List<Waybill> waybills)
        {
            if(waybills == null)
                return null;

            List<WaybillViewModel> result = new List<WaybillViewModel>();

            foreach (var item in waybills)
            {
                result.Add(ViewModelMapper.GetWaybillViewModel(item));
            }

            return result;
        }
    }
}
