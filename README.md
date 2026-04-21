# PreTest
### 마이베네핏 VM 개발팀 입사 지원 사전 과제


최근 AI 툴의 발전으로 인해 단순 코딩 능력보다는, 요구사항에 맞는 적절한 시스템 설계(Architecture)와 코드 구조화 능력, 의도를 중점적으로 확인하고자 합니다.

하기 과제 내용을 수행 후 입사 지원자 git에 업로드 후 git repo를 공유해주시기 바랍니다.

---

### 자판기(Vending Machine) 시스템 구현 사전과제 요구 사항



**1. 데이터 로드 및 초기화** 
- 데이터 소스 : Assets/Resource/Items.json
- 파싱 데이터 : machineid, status, products
---
**2. UI 상태 및 데이터 바인딩**
- 상단 정보(Canvas/Vending Machine/Top/Group)
  - ID Text: machineid 문자열 바인딩
  - Power Light(Image component): status 값에 따라 색상 변경
      - Active: 초록 (0, 255, 0)
      - Inactive: 빨강 (255, 0, 0)
      - Power Light가  Inactive라면 아래 3. 핵심 비즈니스 로직이 동작하지 않고 4. 로그 시스템을 통해 로그 제출
- 상품 목록(Canvas/Vending Machine/Body/ScrollView)
  - 프리팹: Assets/Prefabs/Vending Machine Item
  - 초기화: products 데이터를 기반으로 Vending Machine Item 게임 오브젝트 생성, 리스트 동적 생성 및 데이터 바인딩
  - 제약 조건: 가격 및 잔여 갯수의 원본 string format 유지

---
**3. 핵심 비즈니스 로직**  
- 재화 획득 (Canvas/Optional/Editor)
  - 버튼 클릭 시 명시 된 금액 획득
  - 제약 조건
    - Current Money(Text component) 최대 10,000원 제한
    - Current Money(Text component) 원본 string format 유지
- 상품 구매
  - 상목 목록 - 초기화를 통해 생성 된 Item을 선택 시 금액 차감
  - 프리팹: Assets/Prefabs/Beverage
  - 초기화: Inventory(Canvas/Vending Machine/Inventory/ScrollView) 아이템 생성 및 적재
- 상품 소비
  - Inventory내 Item 선택 시 Item 비표시
---

**4. 로그 시스템**
- 트리거: 재화 획득, 상품 구매, 음료 소비 등 모든 주요 사용자 행위에 대해
- 프리팹: Assets/Prefabs/Log
- Log(Canvas/Optional/Log/ScrollView)내에 하단부터 위로 쌓이도록 UI Layout 구성