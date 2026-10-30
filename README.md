# My2D · 2D 게임 실습 모음

하나의 Unity 프로젝트 안에 2D 액션과 MyBird 실습을 함께 보관합니다.

## 학습·재구현 대상

- 2D 액션: 이동·달리기·점프와 공격
- 2D 액션: 피해·감지 영역과 애니메이션 상태 연결
- MyBird: 플레이어와 장애물 생성 및 점수·UI 관리

## 주요 코드와 구조

| 경로 | 역할 |
| --- | --- |
| [Assets/My2DGame/Scripts](Assets/My2DGame/Scripts) | 2D 액션 코드 |
| [Assets/My2DGame/Scenes/PlayScene.unity](Assets/My2DGame/Scenes/PlayScene.unity) | 2D 액션 씬 |
| [Assets/MyBird/Scripts](Assets/MyBird/Scripts) | MyBird 코드 |
| [Assets/MyBird/Scenes/SampleScene.unity](Assets/MyBird/Scenes/SampleScene.unity) | MyBird 씬 |
| [ProjectSettings/EditorBuildSettings.asset](ProjectSettings/EditorBuildSettings.asset) | 현재 빌드 씬 설정 |

## 실행 방법

1. Unity Hub에서 저장소 폴더를 프로젝트로 추가합니다.
2. **Unity 6000.3.13f1**로 열고 패키지 복원과 임포트를 기다립니다.
3. [시작 씬](Assets/MyBird/Scenes/SampleScene.unity)을 엽니다.
4. Play Mode에서 동작을 확인합니다.

현재 빌드 설정에는 MyBird 씬만 등록되어 있습니다. 2D 액션은 해당 PlayScene을 직접 열어 확인하며 빌드에 포함하려면 빌드 씬 설정을 별도로 확인해야 합니다.

## 학습 방식과 AI 활용

수업 예제를 직접 제작한 뒤 수업에서 사용한 에셋과 관련 자료를 바탕으로 AI를 활용해 같은 기능을 다시 구현하는 학습을 진행했습니다. 수업 실습과 AI를 활용한 재구현 작업이 함께 포함된 학습 과정입니다.

| 구분 | 내용 |
| --- | --- |
| 학습 기반 | 수업에서 다룬 게임 기능과 사용 에셋 |
| AI 활용 | 기존 학습 내용을 바탕으로 기능을 재구현하는 과정에 활용 |
| 자료 성격 | 게임 동작과 구현 구조를 익히기 위한 실습 기록 |

위 기능 목록은 저장소의 코드 구성을 설명합니다. 파일별 수업 예제와 AI 재구현의 구분은 개별 작업 기록을 기준으로 확인합니다.

## 검증과 리소스

실행·빌드 결과는 각 실습 당시의 기록을 기준으로 하며 이 문서의 수정으로 새로운 검증 결과가 추가된 것은 아닙니다. 수업 에셋과 외부 샘플·라이브러리는 각 제공자의 라이선스를 따릅니다.
