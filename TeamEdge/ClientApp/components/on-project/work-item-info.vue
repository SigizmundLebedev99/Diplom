<template>
    <div v-if="currentWI">
        <v-icon class="transparent" @mouseenter="menu=true">info</v-icon>
        <v-menu absolute v-model="menu" :position-x="85" :position-y="55">
            <v-card @mouseleave="menu=false">
                <v-card-text class="pt-2 pb-2">
                    <v-layout column>
                        <v-layout align-center>
                            <span class="mr-3">Добавил: </span>
                            <v-avatar color="primary" size="30">
                                <span v-if="!currentWI.source.description.createdBy.avatar" class="white--text text-xs-center" medium>
                                    {{`${currentWI.source.description.createdBy.fullName.split(' ').map(s=>s[0]).join('')}`}}
                                </span>
                                <v-img v-else :src="currentWI.source.description.createdBy.avatar"/>
                            </v-avatar>
                        </v-layout>
                        <span>Дата добавления: {{getDate(currentWI.source.description.dateOfCreation)}}</span>
                        <v-layout v-if="currentWI.source.description.lastUpdate" column>
                            <v-layout align-center class="mt-2">
                                <span class="mr-3">Изменил: </span>
                                <v-avatar color="primary" size="30">
                                    <span v-if="!currentWI.source.description.lastUpdateBy.avatar" class="white--text text-xs-center" medium>
                                        {{`${currentWI.source.description.lastUpdateBy.fullName.split(' ').map(s=>s[0]).join('')}`}}
                                    </span>
                                    <v-img v-else :src="currentWI.source.description.lastUpdateBy.avatar"/>
                                </v-avatar>
                            </v-layout>
                            <span>Последнее изменение: {{currentWI.source.description.lastUpdate?getDate(currentWI.source.description.lastUpdate):" - "}}</span>
                        </v-layout>
                    </v-layout>
                </v-card-text>
            </v-card>
        </v-menu>
    </div>
</template>

<script>
import { mapGetters } from 'vuex';
export default {
    data:()=>({
        menu:null
    }),
    computed:{
        ...mapGetters({
            currentWI:'project/currentWI'
        })
    },
    methods:{
        getDate(date){
            var temp = new Date(date);
            return `${temp.getDate()}.${temp.getMonth() + 1}.${temp.getFullYear()}`
        }
    }
}
</script>