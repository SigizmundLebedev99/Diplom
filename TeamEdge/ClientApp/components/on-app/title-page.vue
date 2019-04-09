<template>
  <v-container grid-list-md fluid>
    <v-layout column>
      <v-layout row justify-space-around class="mt-5">
          <v-img class="title-img" src="/logos/TE.png" :max-width="ofSize({xs:300,sm:350,md:400})"/>
      </v-layout>
      <v-layout align-center justify-center row fill-height class="mt-5">
        <v-flex md5>
          <blockquote class="blockquote text-xs-center">Team Edge - проект, представляющий собой комплексное решение, объединяющее в себе учет рабочего времени, систему управления версиями, сбор данных, построение отчётов, отслеживание статусов и изменений по проекту и предназначенное для совместной работы над проектами по разработке программного обеспечения.</blockquote>
        </v-flex>
      </v-layout>
    </v-layout>
  </v-container>
</template>

<script>
import onResize from '../../mixins/on-resize'
export default {
  mixins:[onResize]
}
</script>

<style scoped>
.title-img{
    border-radius:12px;
}
</style>




