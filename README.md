# 🎮 AbilityDefense

Unity를 사용하여 개발 중인 모바일 3D 디펜스 게임입니다.

<img width="292/3" height="640/3" src="https://github.com/user-attachments/assets/ebea32cd-938e-47b6-b300-797cb4d64985" alt="Angles Game Screenshot"/>
<img width="288/3" height="640/3" src="https://github.com/user-attachments/assets/b2d7d8cc-1c5a-452a-be08-e6fe7a265c98" alt="Angles Game Screenshot"/>
<img width="264/3" height="640/3" src="https://github.com/user-attachments/assets/456dc5d1-8967-4478-af32-b6f38b992cb1" alt="Angles Game Screenshot"/>

## 📆 개발 기간
2025년 9월 ~ 

## 🧑‍🤝‍🧑 팀 구성
- 1인 개발

## 🛠️ 개발 도구
- Unity (C#)

## 👨‍💻 담당 역할 및 기여도 (기여도 100%)

- ✅ **Flow Field를 적용하여 길찾기 알고리즘 최적화**

---

## 🛠️ Flow Field 적용을 통한 길찾기 알고리즘 최적화

### 문제점 파악 ⚠️

<img width="659" height="371" alt="image" src="https://github.com/user-attachments/assets/6fb33f94-4bc4-43a3-8f1a-768db1108296" />


A* 알고리즘은 **출발 지점부터  목표 지점까지 단방향 경로 탐색 구조**이므로,
객체의 수에 따라 **탐색 횟수가 선형적으로 증가하여 연산 부하가 발생하는 비효율적인 구조입니다.**

### 해결 방법 📈

<img width="264/2" height="640/2" src="https://github.com/user-attachments/assets/456dc5d1-8967-4478-af32-b6f38b992cb1" alt="Angles Game Screenshot"/>

Dijkstra 알고리즘을 활용한 Flow Field를 적용하여 **목표 지점에서 역방향으로 한 번만 탐색**해 전체 지형의 비용과 이동 방향 벡터를 미리 생성합니다.
이후 모든 객체는 해당 방향 정보를 단순 조회만 하면 되므로 개별 경로 탐색 연산이 필요 없습니다.

### 개선 후 Deep Profiling 결과 ⚡

<img width="704" height="346" alt="image" src="https://github.com/user-attachments/assets/9ea207f1-22cc-4de7-a021-e9b615630bf1" />

전체 수행 시간을 **40.23ms → 2.6ms**로 최적화했습니다. 
객체 수가 증가해도 경로 탐색 횟수가 늘지 않아 다수의 객체 이동 상황에서도 안정적인 성능을 유지합니다.
