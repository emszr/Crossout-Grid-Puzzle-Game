using GridManagement;

namespace Utilities.Signals
{
   public class CellInteractedSignal
   {
      public Cell Cell { get; private set; }

      public CellInteractedSignal(Cell cell)
      {
         Cell = cell;
      }
   }
}