### 마이베네핏 VM 개발팀 사전 과제 제출

**개요**
- 기존 제작 되어 있는 UI System에 맞는 구조 설계 및 사전과제 요구사항 구현

### 사전 과제 요구 사항 ###

**구조 설명**
자판기는 MVC 패턴 기반으로 설계, 이벤트 시스템은 제네릭 기반 EventBus를 직접 구현하여 사용
- Model : 비즈니스 로직 자체 처리, View / Controller 에 대한 직접 참조 없음
- View : 데이터 바인딩 및 UI 갱신 담당
- Controller : Model 초기화, EventBus 구독 및 View 연결

**Flow**
Controller → Data Load → VendingMachine / View 생성

사용자 입력 → Controller → VendingMachine → EventBus → Controller / View / LogController 이벤트 실행

**1. 데이터 로드 및 초기화**
- 데이터 소스 : Assets/Resource/Items.json
- 파싱 데이터 : machineid, status, products

데이터 로더용 인터페이스 (IResourceLoader<T>) 로 로드 관련 기능을 추상화 하여 사용
상속 받은 DataLoader 클래스에서 Resources.LoadAsync를 통하여 Json Data를 Load 후 파싱 (JsonUtility 사용)

---

**2. UI 상태 및 데이터 바인딩**
- 상단 정보(Canvas/Vending Machine/Top/Group)
  - ID Text: machineid 문자열 바인딩
  - Power Light(Image component): status 값에 따라 색상 변경
      - Active: 초록 (0, 255, 0)
      - Inactive: 빨강 (255, 0, 0)
      - Power Light가 Inactive라면 아래 3. 핵심 비즈니스 로직이 동작하지 않고 4. 로그 시스템을 통해 로그 제출
- 상품 목록(Canvas/Vending Machine/Body/ScrollView)
  - 프리팹: Assets/Prefabs/Vending Machine Item
  - 초기화: products 데이터를 기반으로 Vending Machine Item 게임 오브젝트 생성, 리스트 동적 생성 및 데이터, Resource 폴더 내 이미지 바인딩
  - 제약 조건: 가격 및 잔여 갯수의 원본 string format 유지

IResourceLoader<T> 를 상속받은 이미지용 ResourcesImageLoader를 구한하여 Resources.LoadAsync로 비동기 로드.
내부에 Dictionary로 캐싱하여 같은 이미지를 중복 로드하지 않도록 구현.
Resources 내 url 경로와 실제 파일 위치가 달라, Controller에서 플래그를 두어 파일명 기준으로 로드하도록 설정, url 경로 방식도 선택 가능하도록 구현

---

**3. 핵심 비즈니스 로직**
- 재화 획득 (Canvas/Optional/Editor)
  - 버튼 클릭 시 명시 된 금액 획득
  - 제약 조건
    - Current Money(Text component) 최대 10,000원 제한
    - Current Money(Text component) 원본 string format 유지
- 상품 구매
  - 상품 목록 - 초기화를 통해 생성 된 Item을 선택 시 금액 차감
  - 프리팹: Assets/Prefabs/Beverage
  - 초기화: Inventory(Canvas/Vending Machine/Inventory/ScrollView) 아이템 생성 및 적재
- 상품 소비
  - Inventory내 Item 선택 시 Item 비표시

VendingMachine 모델을 만들어 해당 모델 내부에서 금액 획득 / 구매 / 소비 로직을 자체 처리하도록 적용
결과 데이터는 EventBus 클래스를 구현하여 해당 클래스에 이벤트를 발행하여 호출하는 형식 적용
구매 후 생성되는 음료 Object는 Unity ObjectPool을 사용하여 관리하며 Active / Release 되는 형태로 구현

---

**4. 로그 시스템**
- 트리거: 재화 획득, 상품 구매, 음료 소비 등 모든 주요 사용자 행위에 대해
- 프리팹: Assets/Prefabs/Log
- Log(Canvas/Optional/Log/ScrollView)내에 하단부터 위로 쌓이도록 UI Layout 구성

VendingMachine 에서 발행한 EventBus 내부의 이벤트 들을 LogController 에서 구독하여 이벤트가 발생할 경우 로그 생성
VendingMachine 에서는 LogEvent의 존재를 모르며, 추후 이벤트 추가 및 Log 관련 이벤트 변경이 필요할 경우 구독하는 대상의 변경으로만 사용이 가능하도록 설계