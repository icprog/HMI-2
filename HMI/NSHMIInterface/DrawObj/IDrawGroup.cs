
namespace NetSCADA6.NSInterface.HMI.DrawObj
{
    public interface IDrawGroup: IDrawVector
    {
        /// <summary>
        /// 父控件刷新标志，为true时，子控件不刷新
        /// </summary>
        bool GroupInvalideate { get; }
    }
}
