using ADJ.DataModel.ShipmentTrack;
using ADJ.Repository.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ADJ.Repository.Interfaces
{
  public interface IManifestRepository : IRepository<Manifest>
  {
    Manifest GetManifestByBookingId(int id);
  }
}
