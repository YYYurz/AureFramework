//------------------------------------------------------------
// AureFramework
// Developed By ZhiRui Yu.
// GitHub: https://github.com/YYYurz
// Gitee: https://gitee.com/yyyurz
// Email: 1228396352@qq.com
//------------------------------------------------------------

using System.Collections.Generic;
using AureFramework.Event;
using AureFramework.ObjectPool;
using AureFramework.Procedure;
using UnityEngine;

namespace GameTest {
	public class ProcedureMain : ProcedureBase {
		private IObjectPool<GameObject> ballObjectPool;
		private readonly HashSet<IObject<GameObject>> recordList = new HashSet<IObject<GameObject>>();
		private Transform spawnPoint;
		private float timeRecord;
		
		
		public override void OnEnter(params object[] args) {
			base.OnEnter(args);

			spawnPoint = GameObject.Find("BallSpawn").transform;
			
			GameMain.Event.Subscribe<BallDropDownEventArgs>(OnBallDropDown);
			
			ballObjectPool = GameMain.ObjectPool.CreateObjectPool<GameObject>("BallPool", 1000, 240);
		}

		public override void OnUpdate() {
			base.OnUpdate();

			timeRecord += Time.deltaTime;
			if (timeRecord < 0.07f) {
				return;
			}
			
			timeRecord = 0f;
			CreateBall();
		}

		private void CreateBall() {
			var obj = ballObjectPool.Spawn();
			if (obj == null) {
				var uiObj = GameMain.Resource.InstantiateSync("Ball");
				obj = ballObjectPool.Register(uiObj, true);
			}

			obj.Target.transform.gameObject.SetActive(true);
			recordList.Add(obj);
		}

		private void OnBallDropDown(object sender, AureEventArgs e) {
			IObject<GameObject> obj = null;
			foreach (var o in recordList) {
				if (o.Target.Equals(sender)) {
					obj = o;
				}
			}

			if (obj != null) {
				obj.Target.SetActive(false);
				obj.Target.transform.position = spawnPoint.position;
				ballObjectPool.Recycle(obj);
				recordList.Remove(obj);
			}
		}
	}
} 