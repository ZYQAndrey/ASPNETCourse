angular.module('ExamApp', [])
    .controller('ExamCtrl', function ($scope, $http) {
        $scope.title = "loading question...";
        $scope.answers = [];
        $scope.answered = false;
        $scope.correctAnswer = false;
        $scope.working = false;
        $scope.score = 0;
        $scope.count = 0;

        $scope.answerClicked = function(answerId) {
            $scope.answers[answerId].selected = true;
        };

        $scope.answer = function () {
            return $scope.correctAnswer ? 'correct' : 'incorrect';
        };

        $scope.nextQuestion = function(examId) {
            $scope.working = true;
            $scope.answered = false;
            $scope.title = "loading question...";
            $scope.answers = [];

            $http.get('/api/exam/' + examId).success(function (data, status, headers, config) {
                $scope.answers = data.answers;
                $scope.answered = false;
                $scope.title = data.description;
                $scope.working = false;
                $scope.quizName = data.quizName;
                $scope.quizId = data.quizId;
                $scope.questionId = data.questionId;
                $scope.startTime = data.startTime;
                $scope.timeLength = data.timeLength;
                $scope.count = $scope.count + 1;
            }).error(function (data, status, headers, config) {
                $scope.title = "Oops... It seems you answered all questions already :)";
                $scope.working = false;
            });
        };

        $scope.sendAnswer = function (answers) {
            $scope.working = true;
            $scope.answered = true;
            
            $http.post('/api/exam', { 'quizId': $scope.quizId, 'questionId': $scope.questionId, 'answers': answers }).success(function (data, status, headers, config) {
                $scope.correctAnswer = (data);
                $scope.working = false;
                if (data === true) {
                    $scope.score = $scope.score + 1;
                }
            }).error(function (data, status, headers, config) {
                $scope.title = "Oops... It seems you answered all questions already :)";
                $scope.working = false;
            });

            $scope.answered = false;
            $scope.title = "loading question...";
            $scope.answers = [];

            $http.get('/api/exam/' + $scope.quizId).success(function (data, status, headers, config) {
                $scope.answers = data.answers;
                $scope.answered = false;
                $scope.title = data.description;
                $scope.working = false;
                $scope.quizName = data.quizName;
                $scope.quizId = data.quizId;
                $scope.questionId = data.questionId;
                $scope.startTime = data.startTime;
                $scope.timeLength = data.timeLength;
                $scope.count = $scope.count + 1;
            }).error(function (data, status, headers, config) {
                $scope.title = "Oops... It seems you answered all questions already :)";
                $scope.working = false;
            });
        };


    });