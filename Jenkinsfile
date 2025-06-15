pipeline {
  agent any
  tools { git 'Default' }

  environment {
    COMPOSE_PROJECT_NAME = 'inkas'
    PATH = "/usr/local/bin:/usr/bin:${env.PATH}"
  }

  stages {
    stage('Checkout') {
      steps {
        deleteDir()
        checkout scm
      }
    }

    stage('Build & Deploy') {
      steps {
        script {
          echo "Deteniendo y eliminando el contenedor users-ms anterior (si existe)…"
          sh "docker-compose -p ${COMPOSE_PROJECT_NAME} -f docker-compose.yml rm -sf users-ms || true"

          echo "Deteniendo y eliminando el contenedor posts-ms anterior (si existe)…"
          sh "docker-compose -p ${COMPOSE_PROJECT_NAME} -f docker-compose.yml rm -sf posts-ms || true"

          echo "Deteniendo y eliminando el contenedor api-gateway anterior (si existe)…"
          sh "docker-compose -p ${COMPOSE_PROJECT_NAME} -f docker-compose.yml rm -sf api-gateway || true"

          echo "Reconstruyendo y desplegando users-ms…"
          sh "docker-compose -p ${COMPOSE_PROJECT_NAME} -f docker-compose.yml up -d --build --no-deps users-ms"

          echo "Reconstruyendo y desplegando posts-ms…"
          sh "docker-compose -p ${COMPOSE_PROJECT_NAME} -f docker-compose.yml up -d --build --no-deps posts-ms"

          echo "Reconstruyendo y desplegando api-gateway…"
          sh "docker-compose -p ${COMPOSE_PROJECT_NAME} -f docker-compose.yml up -d --build --no-deps api-gateway"

          echo "Estado actual de los contenedores:"
          sh "docker-compose -p ${COMPOSE_PROJECT_NAME} -f docker-compose.yml ps"
        }
      }
    }
  }

  post {
    always {
      echo "Build #${env.BUILD_NUMBER} → ${currentBuild.currentResult}"
      sh "docker image prune -f"
    }
    success {
      /*script {
        try {
          mail to:      'juanite.dev@gmail.com',
               subject: "[Jenkins] #${env.BUILD_NUMBER} ${env.JOB_NAME} (SUCCESS)",
               body:    "Detalles: ${env.BUILD_URL}"
        } catch (e) {
          echo "Error enviando correo: ${e.message}"
        }
      }*/
      echo "¡El pipeline se ha completado con exito!"
    }
    failure {
      echo "¡Ha fallado el pipeline!"
    }
  }
}