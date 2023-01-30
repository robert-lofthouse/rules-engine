namespace Domain.RulesEngine.Interface
{
	public interface ICacherService
	{
		T GetValue<T>(string key);

		void SetValue<T>(T item, string key);
	}
}
