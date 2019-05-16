<template>
    <div class="flex-row">
        <v-chip small label :color="workItems[item.code].color" class="white--text center">
            <span class="chip">{{item.code}}-{{item.number}}</span>
        </v-chip>
        <router-link v-if="!withoutRoute" class="ml-2 center" :to="{name:item.code, params:{number:item.number}}">{{item.name}}</router-link>
    </div>
</template>

<script>
import workItems from '../../data/work-items-object'
export default {
    props:[
        'item',
        'withoutRoute'
    ],
    computed:{
        workItems(){
            return workItems;
        }
    }
}
</script>

<style>

</style>
