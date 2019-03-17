<template>
    <v-dialog scrollable v-model="dialog" :width="ofSize({xs:'95%', sm:'70%', md:'60%'})">
        <v-card height="100%">
            <v-toolbar dense color="primary">
                <v-toolbar-title class="white--text">Создание единицы работы</v-toolbar-title>
            </v-toolbar>
            <v-card-text>
                <v-text-field v-model="model.name"></v-text-field>
                <v-text-field v-model="model.descriptionCode"></v-text-field>
            </v-card-text>
        </v-card>
    </v-dialog>
</template>

<script>
import {mapGetters, mapMutations} from 'vuex'
import onResize from '../../mixins/on-resize'
export default {
    mixins:[onResize],
    data:()=>({
        model:{},
    }),
    methods:{
        ...mapMutations({setDialog:'createWorkItem/setDialog'})
    },
    computed:{
        dialog:{
            ...mapGetters({get:'createWorkItem/dialog'}),
            set(value){
                this.setDialog(value);
            }
        }
    }
}
</script>

