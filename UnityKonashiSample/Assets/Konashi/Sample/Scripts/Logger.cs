using UnityEngine;
using System.Text;
using System.Collections.Generic;
using UI = UnityEngine.UI;

namespace Konashi
{
	[RequireComponent(typeof(UI.Text))]
	public class Logger : MonoBehaviour
	{
		public int maxLines = 30;
		
		List<string> logList;
		UI.Text label;
		
		void OnEnable()
		{
			logList = new List<string>();
			label = GetComponent<UI.Text>();
			Application.logMessageReceived += HandlelogMessageReceived;
		}
		
		void OnDisable()
		{
			Application.logMessageReceived -= HandlelogMessageReceived;
		}
		
		void HandlelogMessageReceived (string condition, string stackTrace, LogType type)
		{
			if(type == LogType.Warning) {
				logList.Add(string.Format("<color=yellow>{0}</color>", condition));
			}
			else if(type == LogType.Error){
				logList.Add(string.Format("<color=red>{0}</color>", condition));
			}
			else {
				logList.Add(condition);
			}
			while(logList.Count > maxLines) {
				logList.RemoveAt(0);
			}
			
			var st = new StringBuilder();
			foreach(var log in logList) {
				st.AppendLine(log);
			}
			label.text = st.ToString();
		}
	}
}
