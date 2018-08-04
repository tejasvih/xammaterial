using System.Threading.Tasks;
using System.Threading;


namespace Calibre.Process
{
	public class PlainTaskCounter
    {
		public async Task RunCounter(CancellationToken token)
		{
			await Task.Run (async () => {

				for (long i = 0; i < long.MaxValue; i++) {
					token.ThrowIfCancellationRequested ();

					await Task.Delay(250);
					
				}
			}, token);
		}
	}
}