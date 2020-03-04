using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DIContainer
{
    //DIctionary để chứa các interface và module tương ứng
    public static readonly Dictionary<Type, object>
               ResgisteredModules = new Dictionary<Type, object>();

    public static void SetModule<TInterface, TModule>()
    {
        SetModule(typeof(TInterface), typeof(TModule));
    }

    public static T GetModule<T>()
    {
        return (T)GetModule(typeof(T));
    }


    private static void SetModule(Type interfaceType, Type moduleType)
    {
        if (ResgisteredModules.ContainsKey(interfaceType))
        {
            ResgisteredModules.Remove(interfaceType);
        }
        //Kiểm tra module đã implement interface chưa
        if (!interfaceType.IsAssignableFrom(moduleType))
        {
            throw new Exception("Wrong Module type");

        }
        //Tìm constructor đầu tiên
        var firstConstructor = moduleType.GetConstructors()[0];
        object module = null;
        //Nếu như không có tham số
        if (!firstConstructor.GetParameters().Any())
        {
            //Khởi tạo module
            module = firstConstructor.Invoke(null); // nếu có constructor con khởi tạo dữ liệu bên trong firstCons
            Console.Write(module);
        }
        else
        {
            //Lấy các tham số của constructor
            var constructorParameters = firstConstructor.GetParameters(); //IDatebase, ILogger

            var moduleDependecies = new List<object>();
            foreach (var parameter in constructorParameters)
            {
                var dependency = GetModule(parameter.ParameterType); //Lấy module tương ứng từ DIContainer
                moduleDependecies.Add(dependency);
            }

            //Inject các dependency vào constructor của module
            module = firstConstructor.Invoke(moduleDependecies.ToArray());
            Console.Write(module);
        }
        //Lưu trữ interface và module tương ứng
        ResgisteredModules.Add(interfaceType, module);
    }

    private static object GetModule(Type interfaceType)
    {

        if (ResgisteredModules.ContainsKey(interfaceType))
        {
            return ResgisteredModules[interfaceType];
        }
        throw new Exception(interfaceType + " Module not register");
    }
}