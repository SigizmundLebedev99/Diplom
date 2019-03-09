<template>
    <div>
        <v-layout row justify-space-between align-center :class="ofSize({xs:'mr-1 ml-1 mt-3', sm:'mr-5 mt-3 ml-5'})">
            <span class="title ml-3">
                Ваши проекты
            </span>
            <v-btn>
                <v-icon>add</v-icon>
                <span class="text-none">Создать проект</span>
            </v-btn>
        </v-layout>
        <v-divider></v-divider>
        <v-layout v-if="projects.length === 0" justify-space-around align-center>
            <span class="title ml-3 text-xs-center">
                На данный момент у вы не принимаете участия ни в каком проекте.
            </span>
        </v-layout>
    </div>
</template>

<script>
import {mapGetters} from 'vuex'
import onResize from '../mixins/on-resize'
export default {
    mixins:[onResize],
    computed:{
        ...mapGetters({projects:'projects/projects'})
    }
}
</script>



