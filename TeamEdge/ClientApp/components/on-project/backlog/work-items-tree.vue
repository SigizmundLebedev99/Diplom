<template>
    <v-container class="mx-0 pt-0 scroll-y">
        <v-btn small @click="preWICreating()" class="ml-2 mb-4 text-none">
            <v-icon>
                add
            </v-icon>
            Создать единицу работы
        </v-btn>

        <v-treeview :items="itemsTree" item-key="descriptionId" :open="allIds">
            <template v-slot:label="{ item }">
                <v-layout align-center>
                    <item-chip :item="item"/>
                    <v-spacer/>
                    <v-chip v-if="item.status == 3" label small class="green lighten-1 white--text">Готово</v-chip>
                    <v-chip v-if="item.status == 2" label small class="red lighten-1 white--text">Приостановлено</v-chip>
                    <v-chip label small v-if="item.sprintNumber">Спринт {{item.sprintNumber}}</v-chip>
                </v-layout>
            </template>
        </v-treeview> 

    </v-container>
</template>

<script>
import {mapActions, mapGetters} from 'vuex'
import itemChip from '../item-chip'
export default {
    components:{
        'item-chip':itemChip
    },
    mounted() {
        this.fetchItems();
    },
    methods:{
        ...mapActions({
            preWICreating:'createWorkItem/preWICreating',
            fetchItems:'backlog/fetchItems'
            }),
    },
    computed:{
        allIds(){
            return this.items.map(e=>e.descriptionId)
        },
        ...mapGetters({
            items:'backlog/items',
            itemsTree:'backlog/itemsTree'
        })
    }
}
</script>

<style scoped>
.chip{
    width: 75px;
    text-align: center;
}
</style>

