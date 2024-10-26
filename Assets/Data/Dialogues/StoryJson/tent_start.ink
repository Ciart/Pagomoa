-> startTalk

=== startTalk
#talker 부서진텐트
#sprite Normal
#UIMode In
* [잠자기] -> sleep
* [수행 가능한 퀘스트 보기] -> quest

=== sleep
#UIMode Out
이만 잘까?
*[잘래] -> runSleep
*[아직 안 잘래] -> END

=== runSleep
#start sleep
-> END

=== quest
#start quest
-> END
