using System.Linq;

namespace Common.Util
{
    public class ModelMapUtil
    {
        public static T2 AutoMap<T1, T2>(T1 orignalModel, T2 targetModel)
        {
            var orignalTypeProperties = orignalModel.GetType().GetProperties();
            var targetTypeProperties = targetModel.GetType().GetProperties();
            foreach (var propertyInfo in orignalTypeProperties.Where(e => targetTypeProperties.Select(t => t.Name).Contains(e.Name)))
            {
                var value = propertyInfo.GetValue(orignalModel);
                var targetPropertyInfo = targetTypeProperties.First(t => t.Name == propertyInfo.Name);
                if (targetPropertyInfo.CanWrite)
                    targetPropertyInfo.SetValue(targetModel, value);
            }
            return targetModel;
        }
    }
}
