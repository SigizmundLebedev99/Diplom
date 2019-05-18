<template>
    <v-dialog persistent v-model="dialog" width="300px">
        <v-card v-if="config">
            <v-card-text>
                <p class="text-xs-center">{{config.message}}</p>
            </v-card-text>
            <v-card-actions>
                <v-spacer/>
                <v-btn class="red white--text" @click="config.callback()">Да</v-btn>
                <v-btn class="primary" @click="close()">Нет</v-btn>
                <v-spacer/>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script>
import {mapGetters, mapMutations} from 'vuex'
export default {
    computed:{
        ...mapGetters({
            dialog:'message/dialog',
            config:'message/config'
        })
    },
    methods:{
        ...mapMutations({
            close:'message/close'
        })
    }
}
</script>
