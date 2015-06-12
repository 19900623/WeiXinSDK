using Rabbit.WeiXin.Utility;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Web;

namespace Rabbit.WeiXin.Handlers
{
    /// <summary>
    /// һ������Ĵ��������ġ�
    /// </summary>
    public interface IHandlerContext
    {
        /// <summary>
        /// һ������
        /// </summary>
        HttpRequestBase Request { get; }

        /// <summary>
        /// ������Ӧ��΢�ŵ�Xml���ݡ�
        /// </summary>
        string ResponseXml { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        IDictionary<string, object> Environment { get; }

        /// <summary>
        /// �ӻ����еõ�һ��ֵ��
        /// </summary>
        /// <typeparam name="T">ֵ���͡�</typeparam>
        /// <param name="key">ֵ��key��</param>
        /// <returns>ֵ��</returns>
        T Get<T>(string key);

        /// <summary>
        /// ����һ������ֵ��
        /// </summary>
        /// <typeparam name="T">ֵ���͡�</typeparam>
        /// <param name="key">ֵ��key��</param>
        /// <param name="value">����ֵ��</param>
        /// <returns>���������ġ�</returns>
        IHandlerContext Set<T>(string key, T value);
    }

    /// <summary>
    /// һ��Ĭ�ϵĴ��������ġ�
    /// </summary>
    public sealed class HandlerContext : IHandlerContext
    {
        /// <summary>
        /// ��ʼ��һ���µĴ��������ġ�
        /// </summary>
        /// <param name="request">һ������</param>
        /// <exception cref="ArgumentNullException"><paramref name="request"/> Ϊnull��</exception>
        public HandlerContext(HttpRequest request)
            : this(new HttpRequestWrapper(request.NotNull("request")))
        {
        }

        /// <summary>
        /// ��ʼ��һ���µĴ��������ġ�
        /// </summary>
        /// <param name="request">һ������</param>
        /// <exception cref="ArgumentNullException"><paramref name="request"/> Ϊnull��</exception>
        public HandlerContext(HttpRequestBase request)
        {
            Request = request.NotNull("request");
            Environment = new ConcurrentDictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        #region Implementation of IHandlerContext

        /// <summary>
        /// һ������
        /// </summary>
        public HttpRequestBase Request { get; private set; }

        /// <summary>
        /// ������Ӧ��΢�ŵ�Xml���ݡ�
        /// </summary>
        public string ResponseXml { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public IDictionary<string, object> Environment { get; private set; }

        /// <summary>
        /// �ӻ����еõ�һ��ֵ��
        /// </summary>
        /// <typeparam name="T">ֵ���͡�</typeparam>
        /// <param name="key">ֵ��key�������ִ�Сд����</param>
        /// <returns>ֵ��</returns>
        public T Get<T>(string key)
        {
            object value;
            if (Environment.TryGetValue(key, out value))
                return (T)value;

            return default(T);
        }

        /// <summary>
        /// ����һ������ֵ��
        /// </summary>
        /// <typeparam name="T">ֵ���͡�</typeparam>
        /// <param name="key">ֵ��key�������ִ�Сд����</param>
        /// <param name="value">����ֵ��</param>
        /// <returns>���������ġ�</returns>
        public IHandlerContext Set<T>(string key, T value)
        {
            Environment[key] = value;

            return this;
        }

        #endregion Implementation of IHandlerContext
    }
}