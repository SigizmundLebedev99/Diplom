<template>
     <v-menu v-model="model">
        <template v-slot:activator="{ on }">
            <v-btn icon v-on="on" flat small>
                <v-icon small>{{icon?icon:'edit'}}</v-icon>
            </v-btn>
        </template>
        <div class="constraint white">
            <v-list class="pt-0 pb-0">
                <v-list-tile v-for="(p,i) in project.partisipants" :key="i" @click="$emit('userSelected', p)">
                    <v-list-tile-avatar color="primary" size="35">
                        <span v-if="!p.avatar" class="white--text text-xs-center mt-1" medium>
                            {{`${p.name.split(' ').map(s=>s[0]).join('')}`}}
                        </span>
                        <v-img v-else :src="p.avatar"/>
                    </v-list-tile-avatar>
                    <v-list-tile-title>
                        <span>{{p.name}}</span>
                    </v-list-tile-title>
                </v-list-tile>
            </v-list>
        </div>
    </v-menu>
</template>

<script>
import { mapGetters } from 'vuex';
export default {
    data:()=>({
        model:false
    }),
    props:[
        'icon'
    ],
    computed:{
        ...mapGetters({
            project:'project/project'
        })
    }
}
</script>

<style scoped>
    .constraint{
        max-height: 300px;
    }
</style>

