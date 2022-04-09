﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
	internal class BullshitGenerator
	{
		Random random = new Random();
		String[] set1 = { "Несмотря на замедление падения ремиссионного депрессирования \n",
							"Особенно хотелось отметить то, что \n",
							"Подводя итоги необходимо сказать о том, что \n",
							"Не вызывает сомнений тот факт, что \n",
							"Не может пройти незамеченным то обстоятельство, что \n",
							"В настоящее время главным образом следует обратить внимание на то, что \n",
							"Хотя кадастровая модель и важна, не следует забывать, что \n" };
		String[] set2 = { "субсидирование ассигнационной части бюджетной составляющей \n",
							"рекуперация ротационных секвестров \n",
							"регламентация государственных реестров \n",
							"валоризация базового компонента и варьируемого сегмента \n",
							"ликвидация профицита недодефицитирования \n",
							"уверенное прогрессирование в любом звене указанного диапазона \n",
							"дефлятирование текущих тарифов \n" };
		String[] set3 = { "позволяет уверенно говорить о новой вехе стабилизации \n",
							"обнаруживает тенденцию к устойчивому росту \n",
							"служит толчком к началу нового витка подъёма, который был сформирован \n",
							"является квотой и заделом для повышения общего уровня \n",
							"имеет место быть в каждой сфере деятельности \n",
							"демонстрирует определённый экспоненциальный рывок \n",
							"будет иметь успех только в том случае, когда прирост издержек расходов покажет свою лабильность  \n" };
		String[] set4 = { "за истекший период. \n",
							"на данном этапе развития. \n",
							"как де-юре, так и де- факто. \n",
							"на стадии пилотной сегрегации. \n",
							"после прохождения отчётной фазы. \n",
							"по окончании достагнационного промежутка времени. \n",
							"во всех своих проявлениях. \n" };
		public string getNextSentence()
		{
			return set1[random.Next(set1.Length)] + set2[random.Next(set2.Length)] + set3[random.Next(set3.Length)] + set4[random.Next(set4.Length)];
		}
		public string getGreeting()
		{
			return "Товарищи!\n";
		}
		public string getEnding()
		{
			return "Благодарю за внимание!";
		}
	}
}
