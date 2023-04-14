# 3DPort
 카드의 어빌리티를 활용한 3d 카드 게임

 - 게임 씬은 타이틀 / 로비 / 전투로 구성되어 있습니다.
 - 카드는 드래그하여 캐릭터에게 드롭하면 작동합니다.
 - 현재 게임에 등록된 버프 카드는 아군 캐릭터에게만 / 디버프 카드는 적군 캐릭터에게만 작동합니다.
 - 전투 승리 시, 젬을 획득합니다. 해당 젬으로 상점에서 카드를 구매할 수 있습니다.
 - 전투 패배 시에는 보상이 없습니다.

# 사용한 기술 버전
 - 유니티 버전 : Unity 2021.3.4f1 
 - 추가로 인스톨한 패키지 :
   - Memory Profiler 0.7.1 [ Exp ]
   - Input System 1.3.0 / URP
   - Addressables 1.19.19 
     - 번들 관리에 많은 공수가 예상되어, 어드레서블은 현재 일부 ( 어빌리티 )에만 적용하였습니다.
     - 일부에만 적용된 만큼 implicit asset(암시적 자산)을 피하기 위해 현재 어빌리티에는 이펙트와 메테리얼과 같은 에셋을 포함하지 않고 있습니다.
   
# Battle Scene 구조도
 - 구조도 링크 : https://miro.com/app/board/uXjVMZJqMco=/?share_link_id=918120178507
![Flowchart](https://user-images.githubusercontent.com/66342017/231467730-b2aacffc-c29b-4d27-bd91-92abb35b1629.jpg)
